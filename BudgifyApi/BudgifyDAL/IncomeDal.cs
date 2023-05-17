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
        private readonly BudgetDal _budgetDal;

        public IncomeDal(AppDbContext db, UtilsDal fn, BudgetDal bd)
        {
            _appDbContext = db;
            _utilsDal = fn;
            _budgetDal = bd;
        }
        public async Task<ResponseIncome> CreateIncome(Income newIncome, int wallet_id)
        {
            ResponseIncome response = new ResponseIncome();
            try
            {
                var userid = newIncome.users_id;
                var income = new Income
                {
                    income_id = _utilsDal.GetLastIncomeId() + 1,
                    date = newIncome.date.ToUniversalTime(),
                    status = newIncome.status,
                    users_id = newIncome.users_id,
                    value = newIncome.value,
                    user = _utilsDal.GetUser(userid),
                    wallet = _utilsDal.GetWallet(wallet_id),
                    wallet_id = wallet_id,
                };
                _appDbContext.incomes.Add(income);
                await _appDbContext.SaveChangesAsync();
                var budget = await _budgetDal.updateBudget(userid);
                response.code = true;
                response.message =  "Se creó correctamente el ingreso";
                response.income = Utils.GetIncomeDto(income);
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

        public async Task<ResponseIncome> DeleteIncome(int incomeid)
        {
            ResponseIncome response = new ResponseIncome();
            try
            {
                var income = _appDbContext.incomes.FirstOrDefault(i => i.income_id == incomeid);
                income.wallet = _utilsDal.GetWalletByUserId(income.users_id);
                income.status = "inactive";
                await _appDbContext.SaveChangesAsync();
                var budget = await _budgetDal.updateBudget(income.users_id);
                response.code = true;
                response.message = "Se eliminó correctamente el ingreso";
                response.income = Utils.GetIncomeDto(income);
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

        public async Task<ResponseIncome> ModifyIncome(IncomeDto modifiedIncome, int wallet_id)
        {
            ResponseIncome response = new ResponseIncome();
            try
            {
                var income = _appDbContext.incomes.FirstOrDefault(i => i.income_id == modifiedIncome.income_id);
                income.date = modifiedIncome.date.ToUniversalTime();
                income.value = modifiedIncome.value;
                income.wallet = _utilsDal.GetWallet(wallet_id);
                if (!income.wallet.name.Equals(modifiedIncome.wallet))
                    income.wallet.name = modifiedIncome.wallet;
                await _appDbContext.SaveChangesAsync();
                var budget = await _budgetDal.updateBudget(income.users_id);
                response.code = true;
                response.message = "Se modificó correctamente el ingreso";
                response.income = Utils.GetIncomeDto(income);
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
        public Income[] AsignWalletToIncomes(Income[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                var income = list[i];
                income.wallet = _utilsDal.GetWallet(income.wallet_id);
                list[i] = income;
            }
            return list;
        }
    }
}
