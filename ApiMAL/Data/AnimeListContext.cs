using Microsoft.EntityFrameworkCore;

namespace ApiMAL.Data
{
    public class AnimeListContext : DbContext
    {

        public AnimeListContext(DbSet<AnimeList> _animeList, DbSet<User> user) {
            AnimeList = _animeList;
        }

        public AnimeListContext(DbContextOptions<AnimeListContext> options) : base(options) { }

        public DbSet<AnimeList> AnimeList { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            new DataSeeder(modelBuilder).Seed();
        }
    }
}
