using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyModels
{
    public class Session
    {
        public int UserId { get; set; }
        public Budget? Budget { get; set; }
        public Category[]? Categories { get; set; }
        public Expense[]? Expenses { get; set; }
        public Income[]? Incomes { get; set;}
    }
}
