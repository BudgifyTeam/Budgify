using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BudgifyModels
{
    internal class Category
    {
        [Key]
        [MaxLength(10)]
        public int category_id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? name { get; set; }
    }
}
