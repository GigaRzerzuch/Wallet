using Wallet.Shared.Enums;
using Wallet.Shared.Models;

namespace Wallet.Server.Services
{
    public class CoinsService : ICoinsService
    {
        private readonly HttpClient _httpClient;
        private UriBuilder uriBuilder = new UriBuilder("https://api.coingecko.com/api/v3/simple/price");
        private readonly string[] coingeckoId = {"bitcoin", "ethereum", "usd-coin", "binancecoin", "binance-usd", "cardano", "polkadot", "matic-network", "cosmos",
                                                "decentraland", "algorand", "evmos", "juno-network", "osmosis", "iota", "helium", "e-money-eur", "secret", "agoric",
                                                "stargaze"};

        public CoinsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
    }
}
