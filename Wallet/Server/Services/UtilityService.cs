using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Wallet.Server.Data;
using Wallet.Shared.Models;

namespace Wallet.Server.Services
{
    public class UtilityService :IUtilityService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpAccesor;

        public UtilityService(DataContext context, IHttpContextAccessor httpAccesor)
        {
            _context = context;
            _httpAccesor = httpAccesor;
        }
        public async Task<User> GetUser()
        {
            var userId = int.Parse(_httpAccesor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);
            return user;
        }
    }
}
