using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyModels
{
    public class Budget
    {
        [Key]
        [MaxLength(10)]
        public int Budget_id { get; set; }
        [Required]
        [MaxLength(8)]
        public int Value { get; set; }

        //Relationship
        [ForeignKey("Users_id")]
        public user User { get; set; }
        public int Users_id { get; set; }
    }
}
