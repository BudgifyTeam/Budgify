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
        public int Category_id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }
        //Relationship
        [ForeignKey("Users_id")]
        public user User { get; set; }
        public int Users_id { get; set; }
    }
}
