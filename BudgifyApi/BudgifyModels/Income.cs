using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgifyModels
{
    public class Income
    {
        [Key]
        [Required]
        [MaxLength(10)]
        public int Income_id { get; set; }
        [Required]
        [MaxLength(8)]
        public int Value { get; set; }
        [Required]
        public DateTime Date { get; set; }
        //Relationship
        [ForeignKey("Users_id")]
        public user User { get; set; }
        public int Users_id { get; set; }
    }
}
