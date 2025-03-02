﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ombi.Store.Context.Sqlite;

#nullable disable

namespace Ombi.Store.Migrations.ExternalSqlite
{
    [DbContext(typeof(ExternalSqliteContext))]
    partial class ExternalSqliteContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("Ombi.Store.Entities.CouchPotatoCache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("TheMovieDbId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("CouchPotatoCache");
                });

            modelBuilder.Entity("Ombi.Store.Entities.EmbyContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmbyId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Has4K")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImdbId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Quality")
                        .HasColumnType("TEXT");

                    b.Property<string>("TheMovieDbId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("TvDbId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("EmbyContent");
                });

            modelBuilder.Entity("Ombi.Store.Entities.EmbyEpisode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmbyId")
                        .HasColumnType("TEXT");

                    b.Property<int>("EpisodeNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImdbId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderId")
                        .HasColumnType("TEXT");

                    b.Property<int>("SeasonNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TheMovieDbId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("TvDbId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("EmbyEpisode");
                });

            modelBuilder.Entity("Ombi.Store.Entities.JellyfinContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Has4K")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImdbId")
                        .HasColumnType("TEXT");

                    b.Property<string>("JellyfinId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Quality")
                        .HasColumnType("TEXT");

                    b.Property<string>("TheMovieDbId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("TvDbId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("JellyfinContent");
                });

            modelBuilder.Entity("Ombi.Store.Entities.JellyfinEpisode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("EpisodeNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImdbId")
                        .HasColumnType("TEXT");

                    b.Property<string>("JellyfinId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderId")
                        .HasColumnType("TEXT");

                    b.Property<int>("SeasonNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TheMovieDbId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("TvDbId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("JellyfinEpisode");
                });

            modelBuilder.Entity("Ombi.Store.Entities.LidarrAlbumCache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("ArtistId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ForeignAlbumId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Monitored")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("PercentOfTracks")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<int>("TrackCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("LidarrAlbumCache");
                });

            modelBuilder.Entity("Ombi.Store.Entities.LidarrArtistCache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArtistId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ArtistName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ForeignArtistId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Monitored")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("LidarrArtistCache");
                });

            modelBuilder.Entity("Ombi.Store.Entities.PlexEpisode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EpisodeNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GrandparentKey")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Key")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ParentKey")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SeasonNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GrandparentKey");

                    b.ToTable("PlexEpisode");
                });

            modelBuilder.Entity("Ombi.Store.Entities.PlexSeasonsContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ParentKey")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlexContentId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PlexServerContentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SeasonKey")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SeasonNumber")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PlexServerContentId");

                    b.ToTable("PlexSeasonsContent");
                });

            modelBuilder.Entity("Ombi.Store.Entities.PlexServerContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Has4K")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImdbId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Key")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Quality")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReleaseYear")
                        .HasColumnType("TEXT");

                    b.Property<int?>("RequestId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TheMovieDbId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("TvDbId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PlexServerContent");
                });

            modelBuilder.Entity("Ombi.Store.Entities.RadarrCache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Has4K")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasFile")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasRegular")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TheMovieDbId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("RadarrCache");
                });

            modelBuilder.Entity("Ombi.Store.Entities.SickRageCache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("TvDbId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("SickRageCache");
                });

            modelBuilder.Entity("Ombi.Store.Entities.SickRageEpisodeCache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EpisodeNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SeasonNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TvDbId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("SickRageEpisodeCache");
                });

            modelBuilder.Entity("Ombi.Store.Entities.SonarrCache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("TheMovieDbId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TvDbId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("SonarrCache");
                });

            modelBuilder.Entity("Ombi.Store.Entities.SonarrEpisodeCache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EpisodeNumber")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasFile")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MovieDbId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SeasonNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TvDbId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("SonarrEpisodeCache");
                });

            modelBuilder.Entity("Ombi.Store.Entities.EmbyEpisode", b =>
                {
                    b.HasOne("Ombi.Store.Entities.EmbyContent", "Series")
                        .WithMany("Episodes")
                        .HasForeignKey("ParentId")
                        .HasPrincipalKey("EmbyId");

                    b.Navigation("Series");
                });

            modelBuilder.Entity("Ombi.Store.Entities.JellyfinEpisode", b =>
                {
                    b.HasOne("Ombi.Store.Entities.JellyfinContent", "Series")
                        .WithMany("Episodes")
                        .HasForeignKey("ParentId")
                        .HasPrincipalKey("JellyfinId");

                    b.Navigation("Series");
                });

            modelBuilder.Entity("Ombi.Store.Entities.PlexEpisode", b =>
                {
                    b.HasOne("Ombi.Store.Entities.PlexServerContent", "Series")
                        .WithMany("Episodes")
                        .HasForeignKey("GrandparentKey")
                        .HasPrincipalKey("Key")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Series");
                });

            modelBuilder.Entity("Ombi.Store.Entities.PlexSeasonsContent", b =>
                {
                    b.HasOne("Ombi.Store.Entities.PlexServerContent", null)
                        .WithMany("Seasons")
                        .HasForeignKey("PlexServerContentId");
                });

            modelBuilder.Entity("Ombi.Store.Entities.EmbyContent", b =>
                {
                    b.Navigation("Episodes");
                });

            modelBuilder.Entity("Ombi.Store.Entities.JellyfinContent", b =>
                {
                    b.Navigation("Episodes");
                });

            modelBuilder.Entity("Ombi.Store.Entities.PlexServerContent", b =>
                {
                    b.Navigation("Episodes");

                    b.Navigation("Seasons");
                });
#pragma warning restore 612, 618
        }
    }
}
