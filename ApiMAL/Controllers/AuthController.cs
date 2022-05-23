using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ApiMAL.Controllers
{
    /// <summary>
    /// Controlles to handle the login and registration of an User to use the POST, PUT and DELETE requests in the Anime List
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config) {
            _config = config;
        }

        /// <summary>
        /// Create a New User
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The recent created user data</returns>
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register (UserDTO request) {
            CreatePasswordHash(request.Password, out byte[] passHash, out byte[] passSalt);
            user.PasswordHash = passHash;
            user.PasswordSalt = passSalt;
            user.Username = request.Username;
            return Ok(user);
        }

        /// <summary>
        /// Handle the login of the user and create a Token if the credentials are correct
        /// </summary>
        /// <param name="request"></param>
        /// <returns>An 200 HTTP Code with the created Token or a 400 BadRequest if something went wrong</returns>
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login (UserDTO request) {
            if (user.Username != request.Username) {
                return BadRequest("User not found");
            }
            if (!CheckPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt)) {
                return BadRequest("Wrong Password");
            }
            string token = CreateToken(user);
            return Ok(token);
        }

        /// <summary>
        /// Create a token based on user informatios
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The JWT token</returns>
        private string CreateToken(User user) {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Username)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _config.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        /// <summary>
        /// Check if the password input match the hashed password
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="passHash"></param>
        /// <param name="passSalt"></param>
        /// <returns>True if the passwods match and False if not</returns>
        private bool CheckPasswordHash (string pass, byte[] passHash, byte[] passSalt) {
            using(var hmac = new HMACSHA512(passSalt)) {
                var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
                return hash.SequenceEqual(passHash);
            }
        }

        /// <summary>
        /// Create an encrypted password
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="passHash"></param>
        /// <param name="passSalt"></param>
        private void CreatePasswordHash (string pass, out byte[] passHash, out byte[] passSalt) {
            using(var hmac = new HMACSHA512()) {
                passSalt = hmac.Key;
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
            }
        }
    }
}
