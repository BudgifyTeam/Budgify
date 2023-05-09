using System.ComponentModel.DataAnnotations;

namespace BudgifyModels
{
    public class user
    {
        [Key]
        [Required]
        [MaxLength(10)]
        public int Id { get; set; }
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
    }
}
