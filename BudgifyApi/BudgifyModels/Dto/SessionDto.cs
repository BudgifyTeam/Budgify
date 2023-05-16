using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyModels.Dto
{
    public class SessionDto
    {
        public int UserId { get; set; }
        public string User_icon { get; set; }
        public BudgetDto? Budget { get; set; }
        public CategoryDto[]? Categories { get; set; }
        public ExpenseDto[]? Expenses { get; set; }
        public IncomeDto[]? Incomes { get; set; }
        public PocketDto[]? Pockets { get; set; }
        public WalletDto[]? Wallets { get; set; }
    }
}
