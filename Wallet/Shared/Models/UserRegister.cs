using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Shared.Models
{
    public class UserRegister
    {
        [Required, StringLength(16, ErrorMessage ="User name is to long, max legth 16 characters")]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
