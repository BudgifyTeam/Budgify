using BudgifyModels.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyModels
{
    public class Utils
    {
        public static IncomeDto GetIncomeDto(Income income)
        {
            return new IncomeDto
            {
                value = income.value,
                date = income.date,
                income_id = income.income_id,
                wallet = income.wallet.name
            };
        }

        public static BudgetDto GetBudgetDto(Budget budget)
        {
            return new BudgetDto
            {
                budget_id = budget.budget_id,
                value = budget.value,
            };
        }

        public static CategoryDto GetCategoryDto(Category category)
        {
            return new CategoryDto
            {
                category_id = category.category_id,
                name = category.name,
            };
        }

        public static ExpenseDto GetExpenseDto(Expense expense)
        {
            return new ExpenseDto
            {
                date = expense.date,
                expense_id = expense.expense_id,
                value = expense.value,
                category = expense.category.name,
                wallet = expense.wallet.name
            };
        }
        public static PocketDto GetPocketDto(Pocket pocket)
        {
            return new PocketDto
            {
                goal = pocket.goal,
                icon = pocket.icon,
                name = pocket.name,
                pocket_id = pocket.pocket_id,
                total = pocket.total,
            };
        }
        public static WalletDto GetWalletDto(Wallet wallet)
        {
            return new WalletDto
            {
                total = wallet.total,
                name = wallet.name,
                icon = wallet.icon,
                wallet_id = wallet.wallet_id,
            };
        }
    }
}
