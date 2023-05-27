using BudgifyModels;
using BudgifyModels.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyDal
{
    public class HistoryDal
    {
        private readonly AppDbContext _appDbContext;
        private readonly UtilsDal _utilsDal;

        public HistoryDal(AppDbContext db, UtilsDal fn)
        {
            _appDbContext = db;
            _utilsDal = fn;
        }

        /// <summary>
        /// Retrieves the financial history for a specific user based on the given date and range.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="date">The date for filtering the history.</param>
        /// <param name="range">The range for filtering the history (e.g., "day", "week", "month", "year").</param>
        /// <returns>A ResponseHistory object containing the financial history.</returns>
        public ResponseHistory GetHistory(int userid, DateTime date, string range)
        {
            ResponseHistory response = new ResponseHistory();
            try {
                var incomes = GetIncomesByRange(GetIncomeList(userid), range, date);
                var expenses = GetExpensesByRange(GetExpenseList(userid), range, date);
                response.history = new HistoryDto { 
                    Items = OrderFinancialItemsByDate(GetFinancialItems(incomes, expenses)),
                };
                response.code = true;
            }
            catch (Exception e) {
                response.code = false;
                response.message = e.Message;
            }
            return response;
        }

        /// <summary>
        /// Retrieves a list of FinancialItem objects by combining the given incomes and expenses.
        /// </summary>
        /// <param name="incomes">The list of IncomeDto objects.</param>
        /// <param name="expenses">The list of ExpenseDto objects.</param>
        /// <returns>A list of FinancialItem objects.</returns>
        public List<FinancialItem> GetFinancialItems(List<IncomeDto> incomes, List<ExpenseDto> expenses)
        {
            var list = new List<FinancialItem>(incomes.Count + expenses.Count);

            foreach (var income in incomes)
            {
                var item = CreateFinancialItem("income", income.value, income.date, income.wallet, "none");
                list.Add(item);
            }

            foreach (var expense in expenses)
            {
                var item = CreateFinancialItem("expense", expense.value, expense.date, expense.wallet, expense.category);
                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// Creates a new instance of the FinancialItem class with the specified properties.
        /// </summary>
        /// <param name="name">The name of the financial item.</param>
        /// <param name="value">The value or amount of the financial item.</param>
        /// <param name="date">The date of the financial item.</param>
        /// <param name="type">The type of the financial item.</param>
        /// <param name="category">The category of the financial item.</param>
        /// <returns>A new instance of the FinancialItem class.</returns>
        private FinancialItem CreateFinancialItem(string name, double value, DateTime date, string type, string category)
        {
            return new FinancialItem
            {
                name = name,
                value = value,
                date = date,
                type = type,
                category = category
            };
        }

        /// <summary>
        /// Orders a list of financial items by their date in ascending order.
        /// </summary>
        /// <param name="financialItems">The list of financial items to be ordered.</param>
        /// <returns>The ordered array of financial items.</returns>
        public FinancialItem[] OrderFinancialItemsByDate(List<FinancialItem> financialItems)
        {
            financialItems.Sort((item1, item2) => item1.date.CompareTo(item2.date));
            return financialItems.ToArray();
        }

        /// <summary>
        /// Retrieves a list of income items for a specific user.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <returns>The list of income items for the user.</returns>
        public List<IncomeDto> GetIncomeList(int userid) {
            var incList = _utilsDal.GetIncomesByUserId(userid);
            incList = _utilsDal.AsignWalletToIncomes(incList);
            return incList.Select(Utils.GetIncomeDto).ToList();
        }

        /// <summary>
        /// Retrieves a list of expense items for a specific user.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <returns>The list of expense items for the user.</returns>
        public List<ExpenseDto> GetExpenseList(int userid)
        {
            var expList = _utilsDal.GetExpensesByUserId(userid);
            expList = _utilsDal.AsignCategoryToExpenses(expList);
            expList = _utilsDal.AsignPocketToExpenses(expList);
            expList = _utilsDal.AsignWalletToExpenses(expList);
            return expList.Select(Utils.GetExpenseDto).ToList();
        }

        /// <summary>
        /// Filters a list of expense items based on a specified range and date.
        /// </summary>
        /// <param name="expenses">The list of expense items.</param>
        /// <param name="range">The range to filter the expense items (day, week, month, year).</param>
        /// <param name="date">The reference date for filtering.</param>
        /// <returns>A filtered list of expense items based on the specified range and date.</returns>
        private List<ExpenseDto> GetExpensesByRange(List<ExpenseDto> expenses, string range, DateTime date)
        {
            switch (range)
            {
                case "day":
                    return expenses.Where(i => i.date.Date == date.Date).ToList();
                case "week":
                    var startOfWeek = date.Date.AddDays(-(int)date.DayOfWeek);
                    var endOfWeek = startOfWeek.AddDays(6);
                    return expenses.Where(i => i.date.Date >= startOfWeek && i.date.Date <= endOfWeek).ToList();
                case "month":
                    return expenses.Where(i => i.date.Year == date.Year && i.date.Month == date.Month).ToList();
                case "year":
                    return expenses.Where(i => i.date.Year == date.Year).ToList();
                default:
                    return expenses;
            }
        }

        /// <summary>
        /// Filters a list of income items based on a specified range and date.
        /// </summary>
        /// <param name="incomeList">The list of income items.</param>
        /// <param name="range">The range to filter the income items (day, week, month, year).</param>
        /// <param name="date">The reference date for filtering.</param>
        /// <returns>A filtered list of income items based on the specified range and date.</returns>
        private List<IncomeDto> GetIncomesByRange(List<IncomeDto> incomeList, string range, DateTime date)
        {
            switch (range)
            {
                case "day":
                    return incomeList.Where(i => i.date.Date == date.Date).ToList<IncomeDto>();
                case "week":
                    var startOfWeek = date.Date.AddDays(-(int)date.DayOfWeek);
                    var endOfWeek = startOfWeek.AddDays(6);
                    return incomeList.Where(i => i.date.Date >= startOfWeek && i.date.Date <= endOfWeek).ToList<IncomeDto>();
                case "month":
                    return incomeList.Where(i => i.date.Year == date.Year && i.date.Month == date.Month).ToList<IncomeDto>();
                case "year":
                    return incomeList.Where(i => i.date.Year == date.Year).ToList<IncomeDto>();
                default:
                    return incomeList;
            }
        }

    }
}
