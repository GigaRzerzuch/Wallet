using System;
using System.Net.Http.Json;
using Wallet.Shared.Enums;
using Wallet.Shared.Models;

namespace Wallet.Client.Services
{
    public class CoinService : ICoinService
    {
        private readonly ITradeService _tradeService;
        private readonly HttpClient _http;

        private UriBuilder uriBuilder = new UriBuilder("https://api.coingecko.com/api/v3/simple/price");

        private readonly string[] coingeckoId = {"bitcoin", "ethereum", "usd-coin", "binancecoin", "binance-usd", "cardano", "polkadot", "matic-network", "cosmos",
                                                "decentraland", "algorand", "evmos", "juno-network", "osmosis", "iota", "helium", "e-money-eur", "secret", "agoric",
                                                "stargaze"};

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
            //Enumy CoinName są w tej samej kolejności co coingeckoId
            for (int i = 0; i < Enum.GetNames(typeof(CoinName)).Length; i++)
            {
                foreach (Coin coin in CoinList)
                {
                    if (coin.Name == (CoinName)i)
                    {
                        try
                        {
                            uriBuilder.Query = $"ids={coingeckoId[i]}&vs_currencies=usd";
                            string currentPrice = await _http.GetStringAsync(uriBuilder.Uri);
                            int pFrom = currentPrice.IndexOf("\"usd\":") + "\"usd\":".Length;
                            int pTo = currentPrice.LastIndexOf("}") - 1;
                            currentPrice = currentPrice.Substring(pFrom, pTo - pFrom).Replace('.', ',');
                            coin.CurrentPrice = Double.Parse(currentPrice);
                        }
                        catch (Exception) {}
                    }
                }
            }
        }
    }
}
