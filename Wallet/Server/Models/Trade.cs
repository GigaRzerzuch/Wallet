using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Wallet.Server.Models;

[Index("UserId", Name = "IX_Trades_UserId")]
public partial class Trade
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public int Name { get; set; }

    public int Type { get; set; }

    public DateTime TradeDate { get; set; }

    public double Amount { get; set; }

    public double Price { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Trades")]
    public virtual User User { get; set; } = null!;
}
