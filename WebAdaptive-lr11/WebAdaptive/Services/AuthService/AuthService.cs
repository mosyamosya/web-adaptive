using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAdaptive.Models;

namespace WebAdaptive.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _iconfiguration;

        public AuthService(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        public void SetUserPasswordHash(UserModel user, string password)
        {
            user.HashedPassword = HashPassword(password);
        }

        public string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool VerifyPassword(UserModel user, string password)
        {
            string hashPassword = user.HashedPassword;
            string inputHashPassword = HashPassword(password);

            return (hashPassword == inputHashPassword);
        }

        public TokenModel GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_iconfiguration["JWT:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Name, username),
                    new Claim(JwtRegisteredClaimNames.Aud, _iconfiguration["Jwt:ValidAudience"]),
                    new Claim(JwtRegisteredClaimNames.Iss, _iconfiguration["Jwt:ValidIssuer"])
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var accessToken = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = GenerateRefreshToken();
            return new TokenModel { AccessToken = tokenHandler.WriteToken(accessToken), RefreshToken = refreshToken };
        }


        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[80];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public Task<UserModel> RegisterUser(UserModel newUser)
        {
            SetUserPasswordHash(newUser, newUser.HashedPassword);
            return Task.FromResult(newUser);
        }
    }
}
