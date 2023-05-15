using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyModels.Dto
{
    public class WalletDto
    {
        public int wallet_id { get; set; }
        public string? name { get; set; }
        public double total { get; set; }
        public string? icon { get; set; }
    }
}
