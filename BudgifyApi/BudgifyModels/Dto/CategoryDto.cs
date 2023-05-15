using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyModels.Dto
{
    public class CategoryDto
    {
        public int category_id { get; set; }
        public string? name { get; set; }
    }
}
