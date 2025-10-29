using Wallet.Shared.Enums;

namespace Wallet.Shared.Models
{
    public class Coin
    {
        public CoinName Name { get; set; }
        public double Amount { get; set; } = 0;
        public double CurrentPrice { get; set; } = 0;
        public double BuyPrice { get; set; } = 0;
        public double Sum
        {
            get
            {
                return Amount * CurrentPrice;
            }
        }
        public double Course
        {
            get
            {
                return ((CurrentPrice - BuyPrice) / BuyPrice) * 100;
            }
        }
    }
}
