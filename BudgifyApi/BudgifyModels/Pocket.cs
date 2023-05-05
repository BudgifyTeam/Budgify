using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyModels
{
    internal class Pocket
    {
        [Key]
        [MaxLength(20)]
        public string? pocket_id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? pocket_name { get; set; }
        [Required]
        public double pocket_total { get; set; }
        [Required]
        [MaxLength(100)]
        public string? pocket_icon { get; set; }
        [Required]
        public double pocket_goal { get; set; }
    }
}
