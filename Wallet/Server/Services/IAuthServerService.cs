using Wallet.Shared.Models;

namespace Wallet.Server.Services
{
    public interface IAuthServerService
    {
        public Task<ServiceResponse<int>> Register(User user, string password);
        public Task<ServiceResponse<string>> Login(string email, string password);
    }
}
