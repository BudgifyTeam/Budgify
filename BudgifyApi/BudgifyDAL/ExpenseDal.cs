using BudgifyModels;
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

        public Expense[] AsignWalletToExpenses(Expense[] list)
        {
            throw new NotImplementedException();
        }
        public Expense[] AsignPocketToExpenses(Expense[] list)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseExpense> CreateExpense(Expense newIncome, int wallet_id, int pocket_id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseExpense> DeleteIncome(int expenseid)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseExpense> ModifyExpense(IncomeDto income, int wallet_id, int pocket_id)
        {
            throw new NotImplementedException();
        }
    }
}
