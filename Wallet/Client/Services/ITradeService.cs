using Wallet.Shared.Enums;
using Wallet.Shared.Models;

namespace Wallet.Client.Services
{
    public interface ITradeService
    {
        public List<Trade> Trades { get; set; }
        public List<Trade> PartialTradeList { get; set; }
        public Task AddTradeAsync(Trade trade);
        public Task RemoveTradeAsync(int id);
        public Task GetUserTradesAsync();
        public Task ShowCoinTradesAsync(CoinName name);
        public Task SentTradesAsync();
    }
}
