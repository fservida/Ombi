﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Ombi.Api.Jellyfin;
using Ombi.Api.Jellyfin.Models.Movie;
using Ombi.Core.Settings;
using Ombi.Core.Settings.Models.External;
using Ombi.Helpers;
using Ombi.Hubs;
using Ombi.Schedule.Jobs.Ombi;
using Ombi.Store.Entities;
using Ombi.Store.Repository;
using Quartz;
using MediaType = Ombi.Store.Entities.MediaType;

namespace Ombi.Schedule.Jobs.Jellyfin
{
    public class JellyfinContentSync : IJellyfinContentSync
    {
        public JellyfinContentSync(ISettingsService<JellyfinSettings> settings, IJellyfinApiFactory api, ILogger<JellyfinContentSync> logger,
            IJellyfinContentRepository repo, IHubContext<NotificationHub> notification)
        {
            _logger = logger;
            _settings = settings;
            _apiFactory = api;
            _repo = repo;
            _notification = notification;
        }

        private readonly ILogger<JellyfinContentSync> _logger;
        private readonly ISettingsService<JellyfinSettings> _settings;
        private readonly IJellyfinApiFactory _apiFactory;
        private readonly IJellyfinContentRepository _repo;
        private readonly IHubContext<NotificationHub> _notification;

        private IJellyfinApi Api { get; set; }

        public async Task Execute(IJobExecutionContext job)
        {
            var jellyfinSettings = await _settings.GetSettingsAsync();
            if (!jellyfinSettings.Enable)
                return;
            
            Api = _apiFactory.CreateClient(jellyfinSettings);

            await _notification.Clients.Clients(NotificationHub.AdminConnectionIds)
                .SendAsync(NotificationHub.NotificationEvent, "Jellyfin Content Sync Started");

            foreach (var server in jellyfinSettings.Servers)
            {
                try
                {
                    await StartServerCache(server);
                }
                catch (Exception e)
                {
                    await _notification.Clients.Clients(NotificationHub.AdminConnectionIds)
                        .SendAsync(NotificationHub.NotificationEvent, "Jellyfin Content Sync Failed");
                    _logger.LogError(e, "Exception when caching Jellyfin for server {0}", server.Name);
                }
            }
            await _notification.Clients.Clients(NotificationHub.AdminConnectionIds)
                .SendAsync(NotificationHub.NotificationEvent, "Jellyfin Content Sync Finished");
            // Episodes

            await OmbiQuartz.TriggerJob(nameof(IJellyfinEpisodeSync), "Jellyfin");
        }


        private async Task StartServerCache(JellyfinServers server)
        {
            if (!ValidateSettings(server))
            {
                return;
            }

            if (server.JellyfinSelectedLibraries.Any() && server.JellyfinSelectedLibraries.Any(x => x.Enabled))
            {
                var movieLibsToFilter = server.JellyfinSelectedLibraries.Where(x => x.Enabled && x.CollectionType == "movies");

                foreach (var movieParentIdFilder in movieLibsToFilter)
                {
                    _logger.LogInformation($"Scanning Lib '{movieParentIdFilder.Title}'");
                    await ProcessMovies(server, movieParentIdFilder.Key);
                }

                var tvLibsToFilter = server.JellyfinSelectedLibraries.Where(x => x.Enabled && x.CollectionType == "tvshows");
                foreach (var tvParentIdFilter in tvLibsToFilter)
                {
                    _logger.LogInformation($"Scanning Lib '{tvParentIdFilter.Title}'");
                    await ProcessTv(server, tvParentIdFilter.Key);
                }

                var mixedLibs = server.JellyfinSelectedLibraries.Where(x => x.Enabled && x.CollectionType == "mixed");
                foreach (var m in mixedLibs)
                {
                    _logger.LogInformation($"Scanning Lib '{m.Title}'");
                    await ProcessTv(server, m.Key);
                    await ProcessMovies(server, m.Key);
                }
            }
            else
            {
                await ProcessMovies(server);
                await ProcessTv(server);
            }
        }

        private async Task ProcessTv(JellyfinServers server, string parentId = default)
        {
            // TV Time
            var mediaToAdd = new HashSet<JellyfinContent>();
            var tv = await Api.GetAllShows(server.ApiKey, parentId, 0, 200, server.AdministratorId, server.FullUri);
            var totalTv = tv.TotalRecordCount;
            var processed = 0;
            while (processed < totalTv)
            {
                foreach (var tvShow in tv.Items)
                {
                    try
                    {

                        processed++;
                        if (!tvShow.ProviderIds.Any())
                        {
                            _logger.LogInformation("Provider Id on tv {0} is null", tvShow.Name);
                            continue;
                        }

                        var existingTv = await _repo.GetByJellyfinId(tvShow.Id);
                        if (existingTv == null)
                        {
                            _logger.LogDebug("Adding new TV Show {0}", tvShow.Name);
                            mediaToAdd.Add(new JellyfinContent
                            {
                                TvDbId = tvShow.ProviderIds?.Tvdb,
                                ImdbId = tvShow.ProviderIds?.Imdb,
                                TheMovieDbId = tvShow.ProviderIds?.Tmdb,
                                Title = tvShow.Name,
                                Type = MediaType.Series,
                                JellyfinId = tvShow.Id,
                                Url = JellyfinHelper.GetJellyfinMediaUrl(tvShow.Id, server?.ServerId, server.ServerHostname),
                                AddedAt = DateTime.UtcNow
                            });
                        }
                        else
                        {
                            _logger.LogDebug("We already have TV Show {0}", tvShow.Name);
                        }

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                // Get the next batch
                tv = await Api.GetAllShows(server.ApiKey, parentId, processed, 200, server.AdministratorId, server.FullUri);
                await _repo.AddRange(mediaToAdd);
                mediaToAdd.Clear();
            }

            if (mediaToAdd.Any())
            {
                await _repo.AddRange(mediaToAdd);
            }
        }

        private async Task ProcessMovies(JellyfinServers server, string parentId = default)
        {
            var movies = await Api.GetAllMovies(server.ApiKey, parentId, 0, 200, server.AdministratorId, server.FullUri);
            var totalCount = movies.TotalRecordCount;
            var processed = 0;
            var mediaToAdd = new HashSet<JellyfinContent>();
            var mediaToUpdate = new HashSet<JellyfinContent>();
            while (processed < totalCount)
            {
                foreach (var movie in movies.Items)
                {
                    if (movie.Type.Equals("boxset", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var movieInfo =
                            await Api.GetCollection(movie.Id, server.ApiKey, server.AdministratorId, server.FullUri);
                        foreach (var item in movieInfo.Items)
                        {
                            await ProcessMovies(item, mediaToAdd, mediaToUpdate, server);
                        }

                        processed++;
                    }
                    else
                    {
                        processed++;
                        // Regular movie
                        await ProcessMovies(movie, mediaToAdd, mediaToUpdate, server);
                    }
                }

                // Get the next batch
                movies = await Api.GetAllMovies(server.ApiKey, parentId, processed, 200, server.AdministratorId, server.FullUri);
                await _repo.AddRange(mediaToAdd);
                await _repo.UpdateRange(mediaToUpdate);
                mediaToAdd.Clear();

            }
        }

        private async Task ProcessMovies(JellyfinMovie movieInfo, ICollection<JellyfinContent> content, ICollection<JellyfinContent> toUpdate, JellyfinServers server)
        {
            var quality = movieInfo.MediaStreams?.FirstOrDefault()?.DisplayTitle ?? string.Empty;
            var has4K = false;
            if (quality.Contains("4K", CompareOptions.IgnoreCase))
            {
                has4K = true;
            }

            // Check if it exists
            var existingMovie = await _repo.GetByJellyfinId(movieInfo.Id);
            var alreadyGoingToAdd = content.Any(x => x.JellyfinId == movieInfo.Id);
            if (existingMovie == null && !alreadyGoingToAdd)
            {
                if (!movieInfo.ProviderIds.Any())
                {
                    _logger.LogWarning($"Movie {movieInfo.Name} has no relevant metadata. Skipping.");
                    return;
                }
                _logger.LogDebug($"Adding new movie {movieInfo.Name}");
                content.Add(new JellyfinContent
                {
                    ImdbId = movieInfo.ProviderIds.Imdb,
                    TheMovieDbId = movieInfo.ProviderIds?.Tmdb,
                    Title = movieInfo.Name,
                    Type = MediaType.Movie,
                    JellyfinId = movieInfo.Id,
                    Url = JellyfinHelper.GetJellyfinMediaUrl(movieInfo.Id, server?.ServerId, server.ServerHostname),
                    AddedAt = DateTime.UtcNow,
                    Quality = has4K ? null : quality,
                    Has4K = has4K
                });
            }
            else
            {
                if (!existingMovie.Quality.Equals(quality, StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogDebug($"We have found another quality for Movie '{movieInfo.Name}', Quality: '{quality}'");
                    existingMovie.Quality = has4K ? null : quality;
                    existingMovie.Has4K = has4K;

                    toUpdate.Add(existingMovie);
                }
                else
                {
                    // we have this
                    _logger.LogDebug($"We already have movie {movieInfo.Name}");
                }
            }
        }

        private bool ValidateSettings(JellyfinServers server)
        {
            if (server?.Ip == null || string.IsNullOrEmpty(server?.ApiKey))
            {
                _logger.LogInformation(LoggingEvents.JellyfinContentCacher, $"Server {server?.Name} is not configured correctly");
                return false;
            }

            return true;
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                //_settings?.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
