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
        [ForeignKey("users_id")]
        public user user { get; set; }
        public int users_id { get; set; }
    }
}
