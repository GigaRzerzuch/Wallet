using Wallet.Shared.Models;

namespace Wallet.Server.Services
{
    public interface ICoinsService
    {
        Task<List<Coin>> GetCurrentPrice(List<Coin> coinList);
    }
}
