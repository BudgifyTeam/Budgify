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
        public int value { get; set; }
        [Required]
        public DateTime date { get; set; }

        //Relationship
        
        public int category_id { get; set; }
        [ForeignKey("category_id")]
        public Category category { get; set; }
        public int wallet_id { get; set; }
        [ForeignKey("wallet_id")]
        public Wallet wallet { get; set; }

        [ForeignKey("user_id")]
        public User user { get; set; }
        public int user_id { get; set; }
    }
}
