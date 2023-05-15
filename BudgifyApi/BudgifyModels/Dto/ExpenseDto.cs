using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyModels.Dto
{
    public class ExpenseDto
    {
        public int expense_id { get; set; }
        public int value { get; set; }
        public DateTime date { get; set; }
    }
}
