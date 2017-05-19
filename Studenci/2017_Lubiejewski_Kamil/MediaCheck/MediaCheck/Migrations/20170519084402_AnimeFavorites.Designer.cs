using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MediaCheck.Models;

namespace MediaCheck.Migrations
{
    [DbContext(typeof(Favorites))]
    [Migration("20170519084402_AnimeFavorites")]
    partial class AnimeFavorites
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("MediaCheck.Models.Anime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AiringDate");

                    b.Property<string>("Description");

                    b.Property<string>("EnglishName");

                    b.Property<string>("Genres");

                    b.Property<string>("Image");

                    b.Property<string>("Name");

                    b.Property<int>("NextEpisode");

                    b.Property<string>("OriginalName");

                    b.Property<double>("Ratings");

                    b.Property<int>("TotalEpisodes");

                    b.HasKey("Id");

                    b.ToTable("Anime");
                });

            modelBuilder.Entity("MediaCheck.Models.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Genres");

                    b.Property<string>("Image");

                    b.Property<string>("Name");

                    b.Property<string>("OriginalName");

                    b.Property<double>("Ratings");

                    b.Property<DateTime>("ReleaseDate");

                    b.HasKey("Id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("MediaCheck.Models.Series", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Genres");

                    b.Property<string>("Image");

                    b.Property<string>("Name");

                    b.Property<string>("OriginalName");

                    b.Property<double>("Ratings");

                    b.Property<DateTime>("ReleaseDate");

                    b.HasKey("Id");

                    b.ToTable("Series");
                });
        }
    }
}
