using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaCheck.Migrations
{
    public partial class AnimeFavorites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Anime",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AiringDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    EnglishName = table.Column<string>(nullable: true),
                    Genres = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NextEpisode = table.Column<int>(nullable: false),
                    OriginalName = table.Column<string>(nullable: true),
                    Ratings = table.Column<double>(nullable: false),
                    TotalEpisodes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anime", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Anime");
        }
    }
}
