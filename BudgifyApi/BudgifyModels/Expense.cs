using System.ComponentModel.DataAnnotations;

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
        public int value { get; set; }
        [Required]
        public DateTime date { get; set; }
    }
}
 