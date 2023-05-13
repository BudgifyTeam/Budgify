using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyModels
{
    public class Wallet
    {
        [Key]
        [MaxLength(10)]
        public int Wallet_id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }
        [Required]
        [MaxLength(8)]
        public double Total { get; set; }
        [Required]
        public string? Icon { get; set; }

        //Relationship
        [ForeignKey("Users_id")]
        public user User { get; set; }
        public int Users_id { get; set; }
    }
}
