using System.ComponentModel.DataAnnotations;

namespace BudgifyModels
{
    public class user
    {
        [Key]
        [Required]
        [MaxLength(10)]
        public int users_id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? username { get; set; }
        [Required]
        [MaxLength(50)]
        public string? email { get; set; }
        [Required]
        [MaxLength(100)]
        public string? token { get; set; }
        [Required]
        public bool? status { get; set; }
        [Required]
        public bool? publicaccount { get; set; }
        [Required]
        public string? icon { get; set; }
    }
}
