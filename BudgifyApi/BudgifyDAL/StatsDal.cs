using BudgifyModels;
using BudgifyModels.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyDal
{
    public class StatsDal
    {
        private readonly AppDbContext _appDbContext;
        private readonly UtilsDal _utilsDal;

        public StatsDal(AppDbContext db, UtilsDal fn)
        {
            _appDbContext = db;
            _utilsDal = fn;
        }

        public ResponseCategoryStat GetExpenseByCategory(int userid, DateTime date, string range)
        {
            ResponseCategoryStat response = new ResponseCategoryStat();
            try {
                var categories = _utilsDal.GetCategoriesByUserId(userid).ToList();
                var expenses = GetExpensesByRange(GetExpenseList(userid), range, date);
                if (!expenses.Any())
                {
                    response.code = false;
                    response.message = "el usuario no cuenta con gastos";
                    response.stats = SetListPercentile(GetCategoriesList(expenses, categories));
                }
                else
                {
                    response.stats = SetListPercentile(GetCategoriesList(expenses, categories));
                    response.code = true;
                }
            }
            catch (Exception e) {
                response.code = false;
                response.message = e.Message;
            }
            return response;
        }
        public (StatsCategory[], double) GetCategoriesList(List<ExpenseDto> expenses, List<Category> categories)
        {
            List<StatsCategory> stats = new List<StatsCategory>();
            double bigTotal = 0;

            foreach (Category cat in categories)
            {
                var list = expenses.Where(ex => ex.category == cat.name).ToList();
                double total = list.Sum(ex => ex.value);

                bigTotal += total;

                stats.Add(new StatsCategory
                {
                    name = cat.name,
                    expenses = list.ToArray(),
                    total = total,
                });
            }
            return (stats.ToArray(), bigTotal);
        }

        public StatsCategory[] SetListPercentile((StatsCategory[] list, double total) item)
        {
            var catList = item.list;
            double total = item.total;

            foreach (StatsCategory st in catList)
            {
                if (total == 0) {
                    st.percentile = 0;
                }else {
                    st.percentile = st.total / total;
                } 
            }
            return catList;
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

    }
}
