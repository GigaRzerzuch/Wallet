using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Shared.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Please enter an email address")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
