using Microsoft.EntityFrameworkCore;

namespace ApiMAL.Data
{
    public class DataSeeder : XmlParser
    {
        private readonly AnimeListContext animeListContext;
        private readonly ModelBuilder modelBuilder;

        public DataSeeder (ModelBuilder modelBuilder) {
            this.modelBuilder = modelBuilder;
        }

        public void Seed () {
            var animeList = Parse();
            modelBuilder.Entity<AnimeList>().HasData (animeList);
        }
    }
}
