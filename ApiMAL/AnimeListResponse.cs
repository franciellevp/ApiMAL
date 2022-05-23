namespace ApiMAL
{
    public class AnimeListResponse
    {
        public List<AnimeList> UserList { get; set; }

        public List<uint> UserId { get; set; }

        public int Pages { get; set; }

        public int CurrentPage { get; set; }
    }
}
