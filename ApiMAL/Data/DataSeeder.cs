using Microsoft.EntityFrameworkCore;

namespace ApiMAL.Data
{
    /// <summary>
    /// Class to populate the database
    /// </summary>
    public class DataSeeder : XmlParser
    {
        private readonly ModelBuilder modelBuilder;

        /// <summary>
        /// Construct of the class to construct a model for a context
        /// </summary>
        /// <param name="modelBuilder"></param>
        public DataSeeder (ModelBuilder modelBuilder) {
            this.modelBuilder = modelBuilder;
        }

        /// <summary>
        /// Seed the AnimeList Entity with the XML parsed data
        /// </summary>
        public void Seed () {
            var animeList = Parse();
            modelBuilder.Entity<AnimeList>().HasData (animeList);
        }
    }
}
