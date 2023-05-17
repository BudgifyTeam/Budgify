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

        public Task<ResponseExpense> CreateExpense(int userid, double value, DateTime dateTime, int wallet_id, int pocket_id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseExpense> DeleteExpense(int incomeid)
        {
            throw new NotImplementedException();
        }

        public ResponseList<IncomeDto> GetExpenses(int userid, string range)
        {
            throw new NotImplementedException();
        }

        public ResponseList<IncomeDto> GetExpensesDay(int userid, string v, DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseExpense> ModifyExpense(IncomeDto income, int wallet_id, int pocket_id)
        {
            throw new NotImplementedException();
        }
    }
}
