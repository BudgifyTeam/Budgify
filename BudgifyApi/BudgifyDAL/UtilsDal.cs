using BudgifyModels;
using BudgifyModels.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyDal
{
    public class UtilsDal
    {
        private readonly AppDbContext _appDbContext;

        public UtilsDal(AppDbContext db)
        {
            _appDbContext = db;
        }

        /// <summary>
        /// Creates a new budget for the specified user.
        /// </summary>
        /// <param name="userid">The ID of the user associated with the budget.</param>
        /// <returns>A message indicating the result of the budget creation.</returns>
        public async Task<string> CreateBudget(int userid)
        {
            try
            {
                var newBudget = new Budget
                {
                    budget_id = GetLastBudgetId() + 1,
                    value = 0,
                    users_id = userid,
                    user = GetUser(userid)
                };
                _appDbContext.budget.Add(newBudget);
                await _appDbContext.SaveChangesAsync();
                return " Se creó correctamente el presupuesto";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Creates a new category with the specified parameters.
        /// </summary>
        /// <param name="userid">The ID of the user associated with the category.</param>
        /// <param name="name">The name of the category.</param>
        /// <returns>A response object containing the result of the category creation.</returns>
        public async Task<ResponseCategory> CreateCategory(int userid, string name)
        {
            ResponseCategory response = new ResponseCategory();
            try
            {
                var newCategory = new Category
                {
                    category_id = GetLastCategoryId() + 1,
                    name = name,
                    users_id = userid,
                    user = GetUser(userid),
                    status = "active",
                };
                _appDbContext.categories.Add(newCategory);
                await _appDbContext.SaveChangesAsync();
                response.code = true;
                response.message = "se creó correctamente la categoria";
                response.category = Utils.GetCategoryDto(newCategory);
                return response;
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.code = false;
                return response;
            }
        }

        /// <summary>
        /// Creates a new wallet with the specified parameters.
        /// </summary>
        /// <param name="userid">The ID of the user associated with the wallet.</param>
        /// <param name="name">The name of the wallet.</param>
        /// <param name="icon">The icon associated with the wallet.</param>
        /// <returns>A response object containing the result of the wallet creation.</returns>
        public async Task<ResponseWallet> CreateWallet(int userid, string name, string icon)
        {
            ResponseWallet response = new ResponseWallet();
            try
            {
                var newWallet = new Wallet
                {
                    wallet_id = GetLastWalletId() + 1,
                    name = name,
                    total = 0,
                    icon = icon,
                    status = "active",
                    user = GetUser(userid),
                    users_id = userid,
                };
                _appDbContext.wallets.Add(newWallet);
                await _appDbContext.SaveChangesAsync();
                response.message = " Se creó correctamente la billetera";
                response.code = true;
                response.wallet = Utils.GetWalletDto(newWallet);
            }
            catch (Exception ex)
            {
                response.message = "ocurrió un error al crear la nueva billetera, " + ex.Message;
                response.code = true;
            }
            return response;
        }

        /// <summary>
        /// Creates a new pocket with the specified parameters.
        /// </summary>
        /// <param name="userid">The ID of the user associated with the pocket.</param>
        /// <param name="name">The name of the pocket.</param>
        /// <param name="goal">The goal amount for the pocket.</param>
        /// <param name="icon">The icon associated with the pocket.</param>
        /// <returns>A response object containing the result of the pocket creation.</returns>
        public async Task<ResponsePocket> CreatePocket(int userid, string name, double goal, string icon)
        {
            ResponsePocket response = new ResponsePocket();
            try
            {
                var newPocket = new Pocket
                {
                    pocket_id = GetLastPocketId() + 1,
                    name = name,
                    total = 0,
                    goal = goal,
                    icon = icon,
                    user = GetUser(userid),
                    users_id = userid,
                    status = "active",
                };
                _appDbContext.pockets.Add(newPocket);
                await _appDbContext.SaveChangesAsync(); 
                response.message = " Se creó correctamente el bolsillo";
                response.code = true;
                response.pocket = Utils.GetPocketDto(newPocket);
            }
            catch (Exception ex)
            {
                response.message = "ocurrió un error al crear el nuevo bolsillo, " + ex.Message;
                response.code = true;
            }
            return response;
        }

        /// <summary>
        /// Assigns wallets to incomes in the provided list.
        /// </summary>
        /// <param name="list">The array of incomes to assign wallets to.</param>
        /// <returns>The updated array of incomes with assigned wallets.</returns>
        public Income[] AsignWalletToIncomes(Income[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                var income = list[i];
                income.wallet = GetWallet(income.wallet_id);
                list[i] = income;
            }
            return list;
        }

        /// <summary>
        /// Assigns wallets to expenses in the provided list.
        /// </summary>
        /// <param name="list">The array of expenses to assign wallets to.</param>
        /// <returns>The updated array of expenses with assigned wallets.</returns>
        public Expense[] AsignWalletToExpenses(Expense[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                var expense = list[i];
                expense.wallet = GetWallet(expense.wallet_id);
                list[i] = expense;
            }
            return list;
        }

        /// <summary>
        /// Assigns pockets to expenses in the provided list.
        /// </summary>
        /// <param name="list">The array of expenses to assign pockets to.</param>
        /// <returns>The updated array of expenses with assigned pockets.</returns>
        public Expense[] AsignPocketToExpenses(Expense[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                var expense = list[i];
                expense.pocket = GetPocket(expense.pocket_id);
                list[i] = expense;
            }
            return list;
        }


        /// <summary>
        /// Assigns categories to expenses in the provided list.
        /// </summary>
        /// <param name="list">The array of expenses to assign categories to.</param>
        /// <returns>The updated array of expenses with assigned categories.</returns>
        public Expense[] AsignCategoryToExpenses(Expense[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                var expense = list[i];
                expense.category = GetCategory(expense.category_id);
                list[i] = expense;
            }
            return list;
        }

        /// <summary>
        /// Retrieves a wallet by user ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The wallet associated with the user ID, or null if not found.</returns>
        public Wallet GetWalletByUserId(int id)
        {
            return _appDbContext.wallets.FirstOrDefault(u => u.users_id == id);
        }

        /// <summary>
        /// Retrieves a wallet by ID.
        /// </summary>
        /// <param name="id">The ID of the wallet.</param>
        /// <returns>The wallet with the specified ID, or null if not found.</returns>
        public Wallet GetWallet(int id)
        {
            return _appDbContext.wallets.FirstOrDefault(u => u.wallet_id == id);
        }

        /// <summary>
        /// Retrieves a category by ID.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <returns>The category with the specified ID, or null if not found.</returns>
        public Category GetCategory(int id)
        {
            return _appDbContext.categories.FirstOrDefault(u => u.category_id == id);
        }

        /// <summary>
        /// Retrieves a pocket by ID.
        /// </summary>
        /// <param name="id">The ID of the pocket.</param>
        /// <returns>The pocket with the specified ID, or null if not found.</returns>
        public Pocket GetPocket(int id)
        {
            return _appDbContext.pockets.FirstOrDefault(u => u.pocket_id == id);
        }
        
        /// <summary>
        /// Retrieves the ID of the last income in the database.
        /// </summary>
        /// <returns>The ID of the last income.</returns>
        public int GetLastIncomeId() {
            try
            {
                return _appDbContext.incomes.ToList().OrderByDescending(u => u.income_id).FirstOrDefault().income_id;
            }
            catch
            {
                return 2000002;
            }
        }
        
        /// <summary>
        /// Retrieves the ID of the last expense in the database.
        /// </summary>
        /// <returns>The ID of the last expense.</returns>
        public int GetLastExpenseId() {
            try
            {
                return _appDbContext.expenses.ToList().OrderByDescending(u => u.expense_id).FirstOrDefault().expense_id;
            }
            catch
            {
                return 13000002;
            }
        }
        
        /// <summary>
        /// Retrieves the ID of the last budget in the database.
        /// </summary>
        /// <returns>The ID of the last budget.</returns>
        private int GetLastBudgetId()
        {
            try
            {
                return _appDbContext.budget.ToList().OrderByDescending(u => u.budget_id).FirstOrDefault().budget_id;
            }
            catch
            {
                return 1000001;
            }
        }
        
        /// <summary>
        /// Retrieves the ID of the last category in the database.
        /// </summary>
        /// <returns>The ID of the last category.</returns>
         private int GetLastCategoryId()
        {
            try
            {
                return _appDbContext.categories.ToList().OrderByDescending(u => u.category_id).FirstOrDefault().category_id;
            }
            catch
            {
                return 460000002;
            }
        }

        /// <summary>
        /// Retrieves the ID of the last wallet in the database.
        /// </summary>
        /// <returns>The ID of the last wallet.</returns>
        private int GetLastWalletId()
        {
            try
            {
                return _appDbContext.wallets.ToList().OrderByDescending(u => u.wallet_id).FirstOrDefault().wallet_id;
            }
            catch
            {
                return 35000002;
            }
        }
        
        /// <summary>
        /// Retrieves the ID of the last pocket in the database.
        /// </summary>
        /// <returns>The ID of the last pocket.</returns>
        private int GetLastPocketId()
        {
            try
            {
                return _appDbContext.pockets.ToList().OrderByDescending(u => u.pocket_id).FirstOrDefault().pocket_id;
            }
            catch
            {
                return 24000002;
            }
        }
        
        /// <summary>
        /// Retrieves the user with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The user object.</returns>
        public user GetUser(int id)
        {
            return _appDbContext.users.FirstOrDefault(u => u.users_id == id);
        }
        
        /// <summary>
        /// Retrieves the budget associated with the user ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve the budget for.</param>
        /// <returns>The budget object.</returns>
        public Budget GetBudgetByUserId(int id)
        {
            return _appDbContext.budget.FirstOrDefault(u => u.users_id == id);
        }
        
        /// <summary>
        /// Retrieves the categories associated with the user ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve the categories for.</param>
        /// <returns>An array of categories.</returns>
        public Category[] GetCategoriesByUserId(int id)
        {
            return _appDbContext.categories.Where(c => c.users_id == id && c.status == "active").ToArray();
        }
        
        /// <summary>
        /// Retrieves the expenses associated with the user ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve the expenses for.</param>
        /// <returns>An array of expenses.</returns>
        public Expense[] GetExpensesByUserId(int id)
        {
            return _appDbContext.expenses.Where(c => c.users_id == id && c.status == "active").ToArray();
        }
        
        /// <summary>
        /// Retrieves the incomes associated with the user ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve the incomes for.</param>
        /// <returns>An array of incomes.</returns>
        public Income[] GetIncomesByUserId(int id)
        {
            return _appDbContext.incomes.Where(c => c.users_id == id && c.status == "active").ToArray();
        }

        /// <summary>
        /// Retrieves the pockets associated with the user ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve the pockets for.</param>
        /// <returns>An array of pockets.</returns>
        public Pocket[] GetPocketsByUserId(int id)
        {
            return _appDbContext.pockets.Where(c => c.users_id == id && c.status == "active").ToArray();
        }

        /// <summary>
        /// Retrieves the wallets associated with the user ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve the wallets for.</param>
        /// <returns>An array of wallets.</returns>
        public Wallet[] GetWalletsByUserId(int id)
        {
            return _appDbContext.wallets.Where(c => c.users_id == id && c.status == "active").ToArray();
        }

        /// <summary>
        /// Retrieves the expenses associated with the category ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve the expenses for.</param>
        /// <returns>An array of expenses.</returns>
        public Expense[] GetExpensesByCategory(int id) {
            return _appDbContext.expenses.Where(c => c.category_id == id && c.status == "active").ToArray();
        }

        /// <summary>
        /// Retrieves the incomes associated with the wallet ID.
        /// </summary>
        /// <param name="id">The ID of the wallet to retrieve the incomes for.</param>
        /// <returns>An array of incomes.</returns>
        internal Income[] GetIncomesByWallet(int id)
        {
            return _appDbContext.incomes.Where(c => c.wallet_id == id && c.status == "active").ToArray();
        }

        /// <summary>
        /// Retrieves the expenses associated with the wallet ID.
        /// </summary>
        /// <param name="id">The ID of the wallet to retrieve the expenses for.</param>
        /// <returns>An array of expenses.</returns>
        internal Expense[] GetExpensesByWallet(int id)
        {
            return _appDbContext.expenses.Where(c => c.wallet_id == id && c.status == "active").ToArray();
        }

        /// <summary>
        /// Retrieves the expenses associated with the pocket ID.
        /// </summary>
        /// <param name="id">The ID of the pocket to retrieve the expenses for.</param>
        /// <returns>An array of expenses.</returns>
        internal Expense[] GetExpensesByPocket(int id)
        {
            return _appDbContext.expenses.Where(c => c.pocket_id == id && c.status == "active").ToArray();
        }
    }
}
