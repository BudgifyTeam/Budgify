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
        public int budget_id { get; set; }
        [Required]
        [MaxLength(8)]
        public double value { get; set; }

        //Relationship
        [ForeignKey("users_id")]
        public user user { get; set; }
        public int users_id { get; set; }
    }
}
