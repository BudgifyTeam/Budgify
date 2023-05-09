using System.ComponentModel.DataAnnotations;

namespace BudgifyModels
{
    public class User
    {
        [Key]
        [Required]
        [MaxLength(10)]
        public int user_id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? Username { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Email { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Token { get; set; }
        [Required]
        public bool? Status { get; set; }
        [Required]
        public bool? PublicAccount { get; set; }

        //Relationships
        public Budget budget { get; set; }
        public List<Income> incomes { get; set; }
        public List<Expense> expenses { get; set; }
    }
}
