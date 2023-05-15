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
    public class IncomeBll
    {
        private readonly FinancialDal _financialDal;
        public IncomeBll(AppDbContext db)
        {
            _financialDal = new FinancialDal(db);
        }

        public Task<Response<IncomeDto>> CreateIncome(int userid, double value, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IncomeDto>> DeleteIncome(int userid, int incomeid)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseList<IncomeDto>> GetIncomes(int userid, string range)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseList<IncomeDto>> GetIncomesByDay(int userid, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IncomeDto>> ModifyIncome(IncomeDto income)
        {
            throw new NotImplementedException();
        }
    }
}
