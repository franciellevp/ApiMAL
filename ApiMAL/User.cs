namespace ApiMAL
{
    /// <summary>
    /// User class to save username and encrypted passwords
    /// </summary>
    public class User
    {
        /// <summary>
        /// Constructor of User Class
        /// </summary>
        /// <param name="username"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        public User (string username, byte[] passwordHash, byte[] passwordSalt) {
            Username = username;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public User () {
        }

        public string Username { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
    }
}
