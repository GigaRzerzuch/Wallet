using Wallet.Shared.Enums;
using Wallet.Shared.Models;
using System.Net.Http.Json;
using Blazored.Toast.Services;
using System.Transactions;
using System.Net.Http;

namespace Wallet.Client.Services
{
    public class TradeService : ITradeService
    {
        private readonly HttpClient _http;
        private readonly IToastService _toastService;

        public TradeService(HttpClient http, IToastService toastService)
        {
            _http = http;
            _toastService = toastService;
        }

        public List<Trade> PartialTradeList { get; set; } = new List<Trade>();
        public List<Trade> Trades { get; set; } = new List<Trade>();
        

        public async Task AddTradeAsync(Trade transaction)
        {
            var result = await _http.PostAsJsonAsync<Trade>("api/trade", transaction);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _toastService.ShowError(await result.Content.ReadAsStringAsync());
            }
            else
            {
                _toastService.ShowSuccess($"Your {transaction.Name} transaction has been saved!", "SAVED!");
            }
        }

        public async Task RemoveTradeAsync(int id)
        {
            //status codo handle
            await _http.DeleteAsync($"api/trade/{id}");
        }

        public async Task GetUserTradesAsync()
        {
            Trades = await _http.GetFromJsonAsync<List<Trade>>("api/trade");
        }

        public async Task ShowCoinTradesAsync(CoinName name)
        {
            Trades = await _http.GetFromJsonAsync<List<Trade>>("api/trade");
            Trades = Trades.Where(trade => trade.Name == name).ToList();
        }
    }
}
