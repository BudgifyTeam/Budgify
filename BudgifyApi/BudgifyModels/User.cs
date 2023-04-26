using System.ComponentModel.DataAnnotations;

namespace BudgifyModels
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Username { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }
        [Required]
        public string? Token { get; set; }
        [Required]
        public bool? Status { get; set; }
        [Required]
        public bool? PublicAccount { get; set; }
    }
}
