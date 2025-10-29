using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Wallet.Server.Models;

public partial class User
{
    [Key]
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Trade> Trades { get; set; } = new List<Trade>();
}
