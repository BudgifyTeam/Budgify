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
        private readonly WalletDal _walletDal;

        public IncomeDal(AppDbContext db, UtilsDal fn, BudgetDal bd, WalletDal wd)
        {
            _appDbContext = db;
            _utilsDal = fn;
            _budgetDal = bd;
            _walletDal = wd;
        }

        /// <summary>
        /// Creates a new income and updates the corresponding wallet and budget values.
        /// </summary>
        /// <param name="newIncome">The new income information.</param>
        /// <param name="wallet_id">The ID of the wallet associated with the income.</param>
        /// <returns>A response containing the created income information, updated budget value, and the result of the creation.</returns>
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
                var wallet = await _walletDal.updateWalletValue(income.wallet_id);
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

        /// <summary>
        /// Deletes an income and updates the corresponding wallet and budget values.
        /// </summary>
        /// <param name="incomeid">The ID of the income to be deleted.</param>
        /// <returns>A response containing the deleted income information, updated budget value, and the result of the deletion.</returns>
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
                var wallet = await _walletDal.updateWalletValue(income.wallet_id);
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

        /// <summary>
        /// Modifies an income and updates the corresponding wallet and budget values.
        /// </summary>
        /// <param name="modifiedIncome">The modified income information.</param>
        /// <param name="wallet_id">The ID of the wallet associated with the income.</param>
        /// <returns>A response containing the modified income information, updated budget value, and the result of the modification.</returns>
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
                var wallet = await _walletDal.updateWalletValue(income.wallet_id);
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

        /// <summary>
        /// Assigns the wallet to each income in the provided list.
        /// </summary>
        /// <param name="list">The array of incomes.</param>
        /// <returns>The updated array of incomes with the wallet assigned.</returns>
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
