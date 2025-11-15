using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Wallet.Server.Data;
using Wallet.Shared.Enums;
using Wallet.Shared.Models;

namespace Wallet.Server.Services
{
    public class CoinsService : ICoinsService
    {
        private readonly DataContext _context;
        private readonly IUtilityService _utilityService;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly HttpClient _httpClient;
        private UriBuilder uriBuilder = new UriBuilder("https://api.coingecko.com/api/v3/simple/price");
        private readonly string[] coingeckoId = {"bitcoin", "ethereum", "usd-coin", "binancecoin", "binance-usd", "cardano", "polkadot", "matic-network", "cosmos",
                                                "decentraland", "algorand", "evmos", "juno-network", "osmosis", "iota", "helium", "e-money-eur", "secret", "agoric",
                                                "stargaze"};

        public CoinsService(HttpClient httpClient, ServiceBusClient serviceBusClient, DataContext dataContext, IUtilityService utilityService)
        {
            _httpClient = httpClient;
            _serviceBusClient = serviceBusClient;
            _context = dataContext;
            _utilityService = utilityService;
        }

        public async Task<List<Coin>> GetCurrentPrice(List<Coin> coinList)
        {
            for (int i = 0; i < Enum.GetNames(typeof(CoinName)).Length; i++)
            {
                foreach (Coin coin in coinList)
                {
                    if (coin.Name == (CoinName)i)
                    {
                        try
                        {
                            uriBuilder.Query = $"ids={coingeckoId[i]}&vs_currencies=usd";
                            string currentPrice = await _httpClient.GetStringAsync(uriBuilder.Uri);
                            int pFrom = currentPrice.IndexOf("\"usd\":") + "\"usd\":".Length;
                            int pTo = currentPrice.LastIndexOf("}") - 1;
                            currentPrice = currentPrice.Substring(pFrom, pTo - pFrom).Replace('.', ',');
                            coin.CurrentPrice = Double.Parse(currentPrice);
                        }
                        catch (Exception) { }
                    }
                }
            }

            return coinList;
        }

        public async Task SendTradesMessage()
        {
            var user = await _utilityService.GetUser();
            var trades = await _context.Trades.Where(t => t.UserId == user.Id).ToListAsync();
            var content = JsonConvert.SerializeObject(trades);

            var message = new ServiceBusMessage(content)
            {
                ContentType = "application/json",
                MessageId = Guid.NewGuid().ToString()
            };

            var sender = _serviceBusClient.CreateSender("transactions");
            await sender.SendMessageAsync(message);
        }
    }
}
