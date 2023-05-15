using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyModels.Dto
{
    public class PocketDto
    {
        public int pocket_id { get; set; }
        public string? name { get; set; }
        public double total { get; set; }
        public string? icon { get; set; }
        public double goal { get; set; }
    }
}
