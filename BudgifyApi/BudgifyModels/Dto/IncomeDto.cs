using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyModels.Dto
{
    public class IncomeDto
    {
        public int income_id { get; set; }
        public double value { get; set; }
        public DateTime date { get; set; }
        public string wallet { get; set; }
    }
}
