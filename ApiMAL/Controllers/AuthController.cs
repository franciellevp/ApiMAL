using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ApiMAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config) {
            _config = config;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register (UserDTO request) {
            CreatePasswordHash(request.Password, out byte[] passHash, out byte[] passSalt);
            user.PasswordHash = passHash;
            user.PasswordSalt = passSalt;
            user.Username = request.Username;
            return Ok(user);
        }

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

        private bool CheckPasswordHash (string pass, byte[] passHash, byte[] passSalt) {
            using(var hmac = new HMACSHA512(passSalt)) {
                var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
                return hash.SequenceEqual(passHash);
            }
        }

        private void CreatePasswordHash (string pass, out byte[] passHash, out byte[] passSalt) {
            using(var hmac = new HMACSHA512()) {
                passSalt = hmac.Key;
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
            }
        }
    }
}
