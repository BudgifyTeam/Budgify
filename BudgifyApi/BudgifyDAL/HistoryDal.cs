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
        public FinancialItem[] OrderFinancialItemsByDate(List<FinancialItem> financialItems)
        {
            financialItems.Sort((item1, item2) => item1.date.CompareTo(item2.date));
            return financialItems.ToArray();
        }


        public List<IncomeDto> GetIncomeList(int userid) {
            var incList = _utilsDal.GetIncomesByUserId(userid);
            incList = _utilsDal.AsignWalletToIncomes(incList);
            return incList.Select(Utils.GetIncomeDto).ToList();
        }
        public List<ExpenseDto> GetExpenseList(int userid)
        {
            var expList = _utilsDal.GetExpensesByUserId(userid);
            expList = _utilsDal.AsignCategoryToExpenses(expList);
            expList = _utilsDal.AsignPocketToExpenses(expList);
            expList = _utilsDal.AsignWalletToExpenses(expList);
            return expList.Select(Utils.GetExpenseDto).ToList();
        }

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
