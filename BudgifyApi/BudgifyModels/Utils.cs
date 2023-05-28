using BudgifyModels.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyModels
{

    /// <summary>
    /// Utility class for converting model objects to DTO (Data Transfer Object) objects.
    /// </summary>
    public class Utils
    {

        /// <summary>
        /// Converts an Income object to an IncomeDto object.
        /// </summary>
        /// <param name="income">The Income object to convert.</param>
        /// <returns>The corresponding IncomeDto object.</returns>
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


        /// <summary>
        /// Converts a Budget object to a BudgetDto object.
        /// </summary>
        /// <param name="budget">The Budget object to convert.</param>
        /// <returns>The corresponding BudgetDto object.</returns>
        public static BudgetDto GetBudgetDto(Budget budget)
        {
            return new BudgetDto
            {
                budget_id = budget.budget_id,
                value = budget.value,
            };
        }


        /// <summary>
        /// Converts a Category object to a CategoryDto object.
        /// </summary>
        /// <param name="category">The Category object to convert.</param>
        /// <returns>The corresponding CategoryDto object.</returns>
        public static CategoryDto GetCategoryDto(Category category)
        {
            return new CategoryDto
            {
                category_id = category.category_id,
                name = category.name,
            };
        }


        /// <summary>
        /// Converts an Expense object to an ExpenseDto object.
        /// </summary>
        /// <param name="expense">The Expense object to convert.</param>
        /// <returns>The corresponding ExpenseDto object.</returns>
        public static ExpenseDto GetExpenseDto(Expense expense)
        {
            return new ExpenseDto
            {
                date = expense.date,
                expense_id = expense.expense_id,
                value = expense.value,
                category = expense.category.name,
                wallet = expense.wallet.name,
                pocket = expense.pocket.name
            };
        }

        /// <summary>
        /// Converts a Pocket object to a PocketDto object.
        /// </summary>
        /// <param name="pocket">The Pocket object to convert.</param>
        /// <returns>The corresponding PocketDto object.</returns>
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

        /// <summary>
        /// Converts a Wallet object to a WalletDto object.
        /// </summary>
        /// <param name="wallet">The Wallet object to convert.</param>
        /// <returns>The corresponding WalletDto object.</returns>
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
