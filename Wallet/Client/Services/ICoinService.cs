using Wallet.Shared.Enums;
using Wallet.Shared.Models;

namespace Wallet.Client.Services
{
    public interface ICoinService
    {
        public List<Coin> CoinList { get;}
        public Coin ChoosenCoin { get; set; }
        public Task GetAllCoinsAsync();
    }
}
