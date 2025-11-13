using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Wallet.Server.Data;
using Wallet.Shared.Models;

namespace Wallet.Server.Services
{
    public class AuthServerService : IAuthServerService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthServerService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Email.ToLower().Equals(email.ToLower()));
            if (user == null)
            {
                response.Succes = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Succes = false;
                response.Message = "Wrong pass";
            }
            else
            {
                response.Message = "Logged in";
                response.Data = CreateToken(user);
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            if (await AccountCheck(user.Email))
            {
                return new ServiceResponse<int> { Message = "User already exist.", Succes = false };
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passswordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passswordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new ServiceResponse<int> { Data = user.Id, Message = "Registration succesful!" };
        }

        private async Task<bool> AccountCheck(string email)
        {
            if (await _context.Users.AnyAsync(user => user.Email.ToLower().Equals(email.ToLower())))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }


        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
