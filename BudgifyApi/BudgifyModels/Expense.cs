using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgifyModels
{
    public class Expense
    {
        [Key]
        [Required]
        [MaxLength(10)]
        public int expense_id { get; set; }
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
        [ForeignKey("pocket_id")]
        public Pocket pocket { get; set; }
        public int pocket_id { get; set; }
        [ForeignKey("wallet_id")]
        public Wallet wallet { get; set; }
        public int wallet_id { get; set; }
        [ForeignKey("category_id")]
        public Category category { get; set; }
        public int category_id { get; set; }
    }
}
 