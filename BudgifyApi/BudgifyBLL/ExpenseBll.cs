using BudgifyDal;
using BudgifyModels;
using BudgifyModels.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyBll
{
    public class ExpenseBll
    {
        private readonly UtilsDal _utilsDal;
        private readonly ExpenseDal _expenseDal;
        private readonly BudgetDal _budgifyDal;
        public ExpenseBll(AppDbContext db)
        {
            _utilsDal = new UtilsDal(db);
            _budgifyDal = new BudgetDal(db, _utilsDal);
            _expenseDal = new ExpenseDal(db, _utilsDal, _budgifyDal);
        }

        public async Task<ResponseExpense> CreateExpense(int userid, double value, DateTime date, int wallet_id, int pocket_id, int category_id)
        {
            ResponseExpense response = new ResponseExpense();
            try
            {
                var newExpense = new Expense()
                {
                    date = date,
                    expense_id = 0,
                    status = "active",
                    users_id = userid,
                    value = value
                };
                response = await _expenseDal.CreateExpense(newExpense, wallet_id, pocket_id, category_id);
                if (!response.code)
                {
                    response.message = "Error al registrar al gasto";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseCategory> CreateCategory(int userid, string name)
        {
            ResponseCategory response = new ResponseCategory();
            try
            {
                response = await _expenseDal.CreateCategory(userid, name);
                if (!response.code)
                {
                    response.message = "Error al crear la categoria";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseExpense> DeleteExpense(int expenseid)
        {
            ResponseExpense response = new ResponseExpense();
            try
            {
                response = await _expenseDal.DeleteIncome(expenseid);
                if (!response.code)
                {
                    response.message = "Error al eliminar al gasto";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }

        public ResponseList<ExpenseDto> GetExpenses(int userid, string range)
        {
            var response = new ResponseList<ExpenseDto>();
            try
            {
                var list = _utilsDal.GetExpensesByUserId(userid);
                if (!list.Any())
                {
                    response.message = "El usuario no cuenta con gastos";
                    response.code = false;
                    return response;
                }
                list = _expenseDal.AsignWalletToExpenses(list);
                list = _expenseDal.AsignPocketToExpenses(list);
                list = _expenseDal.AsignCategoryToExpenses(list);
                var expenseList = list.Select(Utils.GetExpenseDto).ToList();
                response.data = GetExpensesByRange(expenseList, range, DateTime.Today).ToList();
                response.message = "Ingresos obtenidos exitosamente";
                response.code = true;
                if (!response.code)
                {
                    response.message = "Error al obtener los ingresos";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.code = false;
            }
            return response;
        }
       
        public ResponseList<ExpenseDto> GetExpensesDay(int userId, string range, DateTime date)
        {
            var response = new ResponseList<ExpenseDto>();
            try
            {
                var list = _utilsDal.GetExpensesByUserId(userId);
                if (!list.Any())
                {
                    response.message = "El usuario no cuenta con gastos";
                    response.code = false;
                    return response;
                }
                list = _expenseDal.AsignWalletToExpenses(list);
                list = _expenseDal.AsignPocketToExpenses(list);
                list = _expenseDal.AsignCategoryToExpenses(list);
                var incomeList = list.Select(Utils.GetExpenseDto).ToList();
                response.data = GetExpensesByRange(incomeList, range, date).ToList();
                response.message = "Gastos obtenidos exitosamente";
                response.code = true;
                if (!response.code)
                {
                    response.message = "Error al obtener los gastos";
                }
                if (response.data.Count == 0)
                {
                    response.message = "No se encontraron gastos para la fecha dada";
                    response.code = false;
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.code = false;
            }
            return response;
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

        public async Task<ResponseExpense> ModifyExpense(ExpenseDto income, int wallet_id, int pocket_id, int categoryid)
        {
            ResponseExpense response = new ResponseExpense();
            try
            {
                response = await _expenseDal.ModifyExpense(income, wallet_id, pocket_id, categoryid);
                if (!response.code)
                {
                    response.message = "Error al modificar al gasto";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }
    }
}
