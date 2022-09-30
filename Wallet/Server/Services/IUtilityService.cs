using Wallet.Shared.Models;

namespace Wallet.Server.Services
{
    public interface IUtilityService
    {
        public Task<User> GetUser();
    }
}
