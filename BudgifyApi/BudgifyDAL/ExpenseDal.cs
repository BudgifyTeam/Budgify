using BudgifyModels;
using BudgifyModels.Dto;

namespace BudgifyDal
{
    public class ExpenseDal
    {
        private readonly AppDbContext _appDbContext;
        private readonly UtilsDal _utilsDal;
        private readonly BudgetDal _budgetDal;
        private readonly WalletDal _walletDal;
        private readonly PocketDal _pocketDal;

        public ExpenseDal(AppDbContext db, UtilsDal fn, BudgetDal bd, WalletDal wd, PocketDal pd)
        {
            _appDbContext = db;
            _utilsDal = fn;
            _budgetDal = bd;
            _walletDal = wd;
            _pocketDal = pd;
        }

        /// <summary>
        /// Creates an expense record.
        /// </summary>
        /// <param name="newExpense">The expense to create.</param>
        /// <param name="wallet_id">The ID of the wallet associated with the expense.</param>
        /// <param name="pocket_id">The ID of the pocket associated with the expense.</param>
        /// <param name="categoryid">The ID of the category associated with the expense.</param>
        /// <returns>A response object containing information about the created expense.</returns>
       public async Task<ResponseExpense> CreateExpense(Expense newExpense, int wallet_id, int pocket_id, int categoryid)
        {
            ResponseExpense response = new ResponseExpense();
            try
            {
                var userid = newExpense.users_id;
                var expense = new Expense
                {
                    category_id = categoryid,
                    category = _utilsDal.GetCategory(categoryid),
                    expense_id = _utilsDal.GetLastExpenseId() + 1,
                    date = newExpense.date.ToUniversalTime(),
                    status = newExpense.status,
                    users_id = newExpense.users_id,
                    value = newExpense.value,
                    user = _utilsDal.GetUser(userid),
                    wallet = _utilsDal.GetWallet(wallet_id),
                    wallet_id = wallet_id,
                    pocket_id = pocket_id,
                    pocket = _utilsDal.GetPocket(pocket_id),
                };
                _appDbContext.expenses.Add(expense);
                await _appDbContext.SaveChangesAsync();
                var budget = await _budgetDal.updateBudget(userid);
                var wallet = await _walletDal.updateWalletValue(expense.wallet_id);
                var pocket = await _pocketDal.updatePocketValue(expense.pocket_id);
                response.code = true;
                response.message = "Se creó correctamente el ingreso";
                response.expense = Utils.GetExpenseDto(expense);
                response.newBudget = budget.data.value;

            }
            catch (Exception ex)
            {
                response.code = false;
                response.message = ex.Message;
                return response;
            }
            return response;
        }

        /// <summary>
        /// Deletes an expense record.
        /// </summary>
        /// <param name="expenseid">The ID of the expense to delete.</param>
        /// <returns>A response object containing information about the deleted expense.</returns>
       public async Task<ResponseExpense> DeleteExpense(int expenseid)
        {
            ResponseExpense response = new ResponseExpense();
            try
            {
                var expense = _appDbContext.expenses.FirstOrDefault(i => i.expense_id == expenseid);
                expense.wallet = _utilsDal.GetWalletByUserId(expense.users_id);
                expense.pocket = _utilsDal.GetPocket(expense.pocket_id);
                expense.category = _utilsDal.GetCategory(expense.category_id);
                expense.status = "inactive";
                await _appDbContext.SaveChangesAsync();
                var budget = await _budgetDal.updateBudget(expense.users_id);
                var wallet = await _walletDal.updateWalletValue(expense.wallet_id);
                var pocket = await _pocketDal.updatePocketValue(expense.pocket_id);
                response.code = true;
                response.message = "Se eliminó correctamente el gasto";
                response.expense = Utils.GetExpenseDto(expense);
                response.newBudget = budget.data.value;
            }
            catch (Exception ex)
            {
                response.code = false;
                response.message = ex.Message;
                return response;
            }

            return response;
        }

        /// <summary>
        /// Modifies an expense record.
        /// </summary>
        /// <param name="ModifiedExpense">The modified expense information.</param>
        /// <param name="wallet_id">The ID of the wallet associated with the expense.</param>
        /// <param name="pocket_id">The ID of the pocket associated with the expense.</param>
        /// <param name="categoryid">The ID of the category associated with the expense.</param>
        /// <returns>A response object containing information about the modified expense.</returns>
        public async Task<ResponseExpense> ModifyExpense(ExpenseDto ModifiedExpense, int wallet_id, int pocket_id, int categoryid)
        {
            ResponseExpense response = new ResponseExpense();
            try
            {
                var expense = _appDbContext.expenses.FirstOrDefault(i => i.expense_id == ModifiedExpense.expense_id);
                expense.date = ModifiedExpense.date.ToUniversalTime();
                expense.value = ModifiedExpense.value;
                expense.wallet = _utilsDal.GetWallet(wallet_id);
                expense.pocket = _utilsDal.GetPocket(pocket_id);
                expense.category = _utilsDal.GetCategory(categoryid);

                //change maybe... (the when the user changes an expense and changes it category the whole thing should change).
                //implement it when managing categories.
                if (!expense.wallet.name.Equals(ModifiedExpense.wallet))
                    expense.wallet.name = ModifiedExpense.wallet;
                if (!expense.pocket.name.Equals(ModifiedExpense.pocket))
                    expense.pocket.name = ModifiedExpense.pocket;
                if (!expense.category.name.Equals(ModifiedExpense.category))
                    expense.category.name = ModifiedExpense.category;

                await _appDbContext.SaveChangesAsync();
                var budget = await _budgetDal.updateBudget(expense.users_id);
                var wallet = await _walletDal.updateWalletValue(expense.wallet_id);
                var pocket = await _pocketDal.updatePocketValue(expense.pocket_id);
                response.code = true;
                response.message = "Se modificó correctamente el gasto";
                response.expense = Utils.GetExpenseDto(expense);
                response.newBudget = budget.data.value;
            }
            catch (Exception ex)
            {
                response.code = false;
                response.message = ex.Message;
                return response;
            }

            return response;
        }
        
        /// <summary>
        /// Assigns wallets to expenses in the provided list.
        /// </summary>
        /// <param name="list">The array of expenses to assign wallets to.</param>
        /// <returns>An array of expenses with assigned wallets.</returns>
        public Expense[] AsignWalletToExpenses(Expense[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                var expense = list[i];
                expense.wallet = _utilsDal.GetWallet(expense.wallet_id);
                list[i] = expense;
            }
            return list;
        }
        
        /// <summary>
        /// Assigns pockets to expenses in the provided list.
        /// </summary>
        /// <param name="list">The array of expenses to assign pockets to.</param>
        /// <returns>An array of expenses with assigned pockets.</returns>
        public Expense[] AsignPocketToExpenses(Expense[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                var expense = list[i];
                expense.pocket = _utilsDal.GetPocket(expense.pocket_id);
                list[i] = expense;
            }
            return list;
        }
        
        /// <summary>
        /// Assigns categories to expenses in the provided list.
        /// </summary>
        /// <param name="list">The array of expenses to assign categories to.</param>
        /// <returns>An array of expenses with assigned categories.</returns>
        public Expense[] AsignCategoryToExpenses(Expense[] list) {
            for (int i = 0; i < list.Length; i++)
            {
                var expense = list[i];
                expense.category = _utilsDal.GetCategory(expense.category_id);
                list[i] = expense;
            }
            return list;
        }

    }
}
