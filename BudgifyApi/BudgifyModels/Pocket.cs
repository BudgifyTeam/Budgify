using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyModels
{
    public class Pocket
    {
        [Key]
        [MaxLength(10)]
        public int pocket_id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? name { get; set; }
        [Required]
        [MaxLength(8)]
        public double total { get; set; }
        [Required]
        public string? icon { get; set; }
        [Required]
        [MaxLength(8)]
        public double goal { get; set; }
        public string status { get; set; }
        //Relationship
        [ForeignKey("users_id")]
        public user user { get; set; }
        public int users_id { get; set; }
    }
}
