using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MediaCheck.Models;

namespace MediaCheck.Migrations
{
    [DbContext(typeof(Favorites))]
    [Migration("20170518081503_SettingUpDB")]
    partial class SettingUpDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

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
