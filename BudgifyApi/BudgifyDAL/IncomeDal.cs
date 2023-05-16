using BudgifyModels.Dto;
using BudgifyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyDal
{
    public class IncomeDal
    {
        private readonly AppDbContext _appDbContext;
        private readonly UtilsDal _utilsDal;

        public IncomeDal(AppDbContext db, UtilsDal fn)
        {
            _appDbContext = db;
            _utilsDal = fn;
        }
        public async Task<Response<IncomeDto>> CreateIncome(Income newIncome)
        {
            Response<IncomeDto> response = new Response<IncomeDto>();
            try
            {
                var userid = newIncome.users_id;
                var wallet = _utilsDal.GetWalletByUserId(userid);
                var income = new Income
                {
                    income_id = _utilsDal.GetLastIncomeId() + 1,
                    date = newIncome.date,
                    status = newIncome.status,
                    users_id = newIncome.users_id,
                    value = newIncome.value,
                    user = _utilsDal.GetUser(userid),
                    wallet = wallet,
                    wallet_id = wallet.wallet_id,
                };
                _appDbContext.incomes.Add(income);
                await _appDbContext.SaveChangesAsync();
                response.code = true;
                response.message =  "Se creó correctamente el ingreso";
            }
            catch (Exception ex)
            {
                response.code = false;
                response.message = ex.Message;
                return response;
            }
            return response;
        }

        public Task<Response<IncomeDto>> DeleteIncome(int userid, int incomeid)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IncomeDto>> ModifyIncome(IncomeDto income)
        {
            throw new NotImplementedException();
        }
    }
}
