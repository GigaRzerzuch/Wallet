using Wallet.Shared.Models;

namespace Wallet.Client.Services
{
    public interface IAuthService
    {
        public Task<ServiceResponse<string>> Login(UserLogin user);
        public Task<ServiceResponse<int>> Register(UserRegister user);
    }
}
