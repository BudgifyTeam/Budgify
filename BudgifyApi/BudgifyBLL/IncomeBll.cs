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

        public async Task<Response<IncomeDto>> CreateIncome(int userid, double value, DateTime date)
        {
           Response<IncomeDto> response = new Response<IncomeDto>();
            try {
                var newIncome = new Income()
                {
                    date = date,
                    income_id = 0,
                    status = "active",
                    users_id = userid,
                    value = value
                };
                response = await _financialDal.CreateIncome(newIncome);
                if (!response.code)
                {
                    response.message = "Error al registrar al ingreso";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<Response<IncomeDto>> DeleteIncome(int userid, int incomeid)
        {
            Response<IncomeDto> response = new Response<IncomeDto>();
            try
            {
                response = await _financialDal.DeleteIncome(userid, incomeid);
                if (!response.code)
                {
                    response.message = "Error al eliminar al ingreso";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }

        public Task<ResponseList<IncomeDto>> GetIncomes(int userid, string range)//range{day, week, month, year}
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
