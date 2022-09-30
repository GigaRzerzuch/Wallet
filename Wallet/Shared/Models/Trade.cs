using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Shared.Enums;

namespace Wallet.Shared.Models
{
    public class Trade
    {
        public int Id { get; set; }
        public int UserId { get; set; } = 999999;
        public CoinName Name { get; set; }
        public TradeType Type { get; set; }
        public DateTime TradeDate { get; set; } = DateTime.Now;
        public double Amount { get; set; }
        public double Price { get; set; }
        public double TotalPrice {
            get {
                return Price * Amount;
            } 
        }
    }
}
