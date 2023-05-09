using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyModels
{
    internal class Budget
    {
        [Key]
        [MaxLength(10)]
        public int budget_id { get; set; }
        [Required]
        [MaxLength(8)]
        public int value { get; set; }

        //Relationship
        public user User { get; set; }
    }
}
