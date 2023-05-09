using System.ComponentModel.DataAnnotations;

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


    }
}
