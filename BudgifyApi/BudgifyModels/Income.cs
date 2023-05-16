using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgifyModels
{
    public class Income
    {
        [Key]
        [Required]
        [MaxLength(10)]
        public int income_id { get; set; }
        [Required]
        [MaxLength(8)]
        public double value { get; set; }
        [Required]
        public DateTime date { get; set; }
        [Required]
        [MaxLength(10)]
        public string status { get; set; }
        //Relationship
        [ForeignKey("users_id")]
        public user user { get; set; }
        public int users_id { get; set; }
        [ForeignKey("wallet_id")]
        public Wallet wallet { get; set; }
        public int wallet_id { get; set; }
    }
}
