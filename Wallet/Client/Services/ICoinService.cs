using Wallet.Shared.Models;

namespace Wallet.Client.Services
{
    public interface ICoinService
    {
        public List<Coin> CoinList { get; set; }
        public Coin ChoosenCoin { get; set; }
        public Task GetAllCoinsAsync();
    }
}
