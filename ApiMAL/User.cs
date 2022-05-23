namespace ApiMAL
{
    public class User
    {
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
