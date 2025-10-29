using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using Wallet.Shared.Enums;
using Wallet.Shared.Models;

namespace Wallet.Client.Services
{
    public class CoinService : ICoinService
    {
        private readonly ITradeService _tradeService;
        private readonly HttpClient _http;
        public Coin ChoosenCoin { get; set; } = new Coin();
        public List<Coin> CoinList { get; set; } = new List<Coin>();

        public CoinService(ITradeService tradeService, HttpClient http)
        {
            _tradeService = tradeService;
            _http = http;
        }

        public async Task GetAllCoinsAsync()
        {
            await _tradeService.GetUserTradesAsync();
            await InitializeCoinList();
            await GetCurrentPrice();
        }

        private async Task InitializeCoinList()
        {
            List<Coin> dummyList = new List<Coin>();
            CoinList.Clear();

            foreach (CoinName coinName in CoinName.GetValues(typeof(CoinName)))
            {
                dummyList.Add(new Coin { Name = coinName });
            }

            foreach (Coin dummyCoin in dummyList)
            {
                foreach (Trade trade in _tradeService.Trades)
                {
                    if (dummyCoin.Name == trade.Name)
                    {
                        if (trade.Type == TradeType.Buy)
                        {
                            dummyCoin.BuyPrice = (dummyCoin.BuyPrice * dummyCoin.Amount + trade.Price * trade.Amount) / (dummyCoin.Amount + trade.Amount);
                            dummyCoin.Amount += trade.Amount;
                        }
                        else if (trade.Type == TradeType.Sell)
                        {
                            dummyCoin.BuyPrice = (dummyCoin.BuyPrice * dummyCoin.Amount - trade.Price * trade.Amount) / (dummyCoin.Amount - trade.Amount);
                            dummyCoin.Amount -= trade.Amount;
                        }
                        else if (trade.Type == TradeType.Claim)
                        {
                            dummyCoin.BuyPrice = (dummyCoin.BuyPrice * dummyCoin.Amount) / (dummyCoin.Amount + trade.Amount);
                            dummyCoin.Amount += trade.Amount;
                        }
                    }
                }
            }

            foreach (Coin dummyCoin in dummyList)
            {
                if (dummyCoin.Amount > 0)
                {
                    CoinList.Add(dummyCoin);
                }
            }
        }

        private async Task GetCurrentPrice()
        {
            var response = await _http.PostAsJsonAsync("api/coin", CoinList);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                CoinList = JsonConvert.DeserializeObject<List<Coin>>(await response.Content.ReadAsStringAsync());

                Console.WriteLine(JsonConvert.SerializeObject(CoinList));
            }
        }
    }
}
