using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgifyModels
{
    public class Expense
    {
        [Key]
        [Required]
        [MaxLength(10)]
        public int Expense_id { get; set; }
        [Required]
        [MaxLength(8)]
        public int Value { get; set; }
        [Required]
        public DateTime Date { get; set; }
        //Relationship
        [ForeignKey("Users_id")]
        public user User { get; set; }
        public int Users_id { get; set; }
        [ForeignKey("Pocket_id")]
        public Pocket Pocket { get; set; }
        public int Pocket_id { get; set; }
        [ForeignKey("Wallet_id")]
        public Pocket Wallet { get; set; }
        public int Wallet_id { get; set; }
    }
}
 