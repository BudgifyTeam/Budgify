using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyModels
{
    internal class Wallet
    {
        [Key]
        [MaxLength(20)]
        public string? wallet_id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? wallet_name { get; set; }
        [Required]
        public double wallet_total { get; set; }
    }
}
