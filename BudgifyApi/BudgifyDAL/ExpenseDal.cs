﻿using BudgifyModels;
using BudgifyModels.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyDal
{
    public class ExpenseDal
    {
        private readonly AppDbContext _appDbContext;
        private readonly UtilsDal _utilsDal;
        private readonly BudgetDal _budgetDal;

        public ExpenseDal(AppDbContext db, UtilsDal fn, BudgetDal bd)
        {
            _appDbContext = db;
            _utilsDal = fn;
            _budgetDal = bd;
        }

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

        public async Task<ResponseExpense> DeleteIncome(int expenseid)
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
