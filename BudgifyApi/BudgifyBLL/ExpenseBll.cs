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
    /// <summary>
    /// Represents the business logic layer for managing expense-related operations.
    /// </summary>
    public class ExpenseBll
    {
        private readonly UtilsDal _utilsDal;
        private readonly ExpenseDal _expenseDal;
        private readonly BudgetDal _budgifyDal;
        private readonly WalletDal _walletDal;
        private readonly PocketDal _pocketDal;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseBll"/> class with the specified <see cref="AppDbContext"/> object.
        /// </summary>
        /// <param name="db">The database context.</param>
        public ExpenseBll(AppDbContext db)
        {
            _utilsDal = new UtilsDal(db);
            _budgifyDal = new BudgetDal(db, _utilsDal);
            _walletDal = new WalletDal(db, _utilsDal);
            _pocketDal = new PocketDal(db, _utilsDal);
            _expenseDal = new ExpenseDal(db, _utilsDal, _budgifyDal, _walletDal, _pocketDal);
        }

        /// <summary>
        /// Creates a new expense for a user.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="value">The value of the expense.</param>
        /// <param name="date">The date of the expense.</param>
        /// <param name="wallet_id">The ID of the wallet associated with the expense.</param>
        /// <param name="pocket_id">The ID of the pocket associated with the expense.</param>
        /// <param name="category_id">The ID of the category associated with the expense.</param>
        /// <returns>A <see cref="Task{ResponseExpense}"/> representing the asynchronous operation, containing the response for the expense creation.</returns>
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

        /// <summary>
        /// Deletes an expense.
        /// </summary>
        /// <param name="expenseid">The ID of the expense to delete.</param>
        /// <returns>A <see cref="Task{ResponseExpense}"/> representing the asynchronous operation, containing the response for the expense deletion.</returns>
        public async Task<ResponseExpense> DeleteExpense(int expenseid)
        {
            ResponseExpense response = new ResponseExpense();
            try
            {
                response = await _expenseDal.DeleteExpense(expenseid);
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

        /// <summary>
        /// Retrieves a list of expenses for a user within the specified range.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="range">The range for filtering the expenses (e.g., day, week, month, year).</param>
        /// <returns>A <see cref="ResponseList{ExpenseDto}"/> representing the response containing the list of expenses.</returns>
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

        /// <summary>
        /// Retrieves a list of expenses for a user within the specified range and date.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="range">The range for filtering the expenses (e.g., day, week, month, year).</param>
        /// <param name="date">The date for filtering the expenses.</param>
        /// <returns>A <see cref="ResponseList{ExpenseDto}"/> representing the response containing the list of expenses.</returns>
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

        /// <summary>
        /// Filters a list of expenses based on the specified range and date.
        /// </summary>
        /// <param name="expenses">The list of expenses to filter.</param>
        /// <param name="range">The range for filtering the expenses (e.g., day, week, month, year).</param>
        /// <param name="date">The date for filtering the expenses.</param>
        /// <returns>A filtered list of expenses based on the specified range and date.</returns>
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
        /// Modifies an existing expense.
        /// </summary>
        /// <param name="income">The modified expense data.</param>
        /// <param name="wallet_id">The ID of the wallet associated with the expense.</param>
        /// <param name="pocket_id">The ID of the pocket associated with the expense.</param>
        /// <param name="categoryid">The ID of the category associated with the expense.</param>
        /// <returns>A <see cref="Task{ResponseExpense}"/> representing the asynchronous operation, containing the response for the expense modification.</returns>
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
