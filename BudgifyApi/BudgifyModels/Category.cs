using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgifyModels
{
    public class Category
    {
        [Key]
        [MaxLength(10)]
        public int category_id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? name { get; set; }
        //Relationship
        [ForeignKey("users_id")]
        public user user { get; set; }
        public int users_id { get; set; }
    }
}
