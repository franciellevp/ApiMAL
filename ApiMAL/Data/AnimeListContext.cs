using Microsoft.EntityFrameworkCore;

namespace ApiMAL.Data
{
    /// <summary>
    /// Class to create the database session for querying and saving data
    /// </summary>
    public class AnimeListContext : DbContext
    {

        /// <summary>
        /// Constructor of the class that receives the entity for querying and saving data
        /// </summary>
        /// <param name="_animeList"></param>
        public AnimeListContext (DbSet<AnimeList> _animeList) {
            AnimeList = _animeList;
        }

        public AnimeListContext (DbContextOptions<AnimeListContext> options) : base(options) { }

        public DbSet<AnimeList> AnimeList { get; set; }

        /// <summary>
        /// Method called when creating the current model to populate the database with the XML file
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            new DataSeeder(modelBuilder).Seed();
        }
    }
}
