namespace ApiMAL
{
    /// <summary>
    /// Class to specify the information for Pagination
    /// </summary>
    public class AnimeListResponse
    {
        /// <summary>
        /// A list containing all the data of an user anime list
        /// </summary>
        public List<AnimeList> UserList { get; set; }

        /// <summary>
        /// A list containing all the users ids of the database
        /// </summary>
        public List<uint> UserId { get; set; }

        public int Pages { get; set; }

        public int CurrentPage { get; set; }
    }
}
