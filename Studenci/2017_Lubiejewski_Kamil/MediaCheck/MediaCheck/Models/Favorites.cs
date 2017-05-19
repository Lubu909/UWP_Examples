using Microsoft.EntityFrameworkCore;

namespace MediaCheck.Models {
    public class Favorites : DbContext {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Anime> Anime { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=fav.db");
        }
    }
}
