using BudgifyModels;
using BudgifyModels.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BudgifyDal
{
    public class WalletDal
    {
        private readonly AppDbContext _appDbContext;
        private readonly UtilsDal _utilsDal;
        private readonly ExpenseDal _expenseDal;
        private readonly IncomeDal _incomeDal;

        public WalletDal(AppDbContext db, UtilsDal utils)
        {
            _appDbContext = db;
            _utilsDal = utils;
        }
        /// <summary>
        /// Updates the total value of a wallet.
        /// </summary>
        /// <param name="wallet_id">The ID of the wallet to update.</param>
        /// <returns>A response object containing the updated wallet information.</returns>
        public async Task<Response<Wallet>> updateWalletValue(int wallet_id)
        {
            Response<Wallet> response = new Response<Wallet>();
            try
            {
                var wallet = _appDbContext.wallets.FirstOrDefault(b => b.wallet_id == wallet_id);
                var incomes = _utilsDal.GetIncomesByWallet(wallet_id);
                var expenses = _utilsDal.GetExpensesByWallet(wallet_id);
                if (expenses.Any() && incomes.Any())
                    wallet.total = incomes.Sum(i => i.value) - expenses.Sum(i => i.value);
                if (!expenses.Any())
                {
                    if (!incomes.Any())
                    {
                        wallet.total = 0;
                        await _appDbContext.SaveChangesAsync();
                        response.data = wallet;
                        response.code = true;
                        response.message = "se actualizó el total de la cartera correctamente";
                        return response;
                    }
                    wallet.total = incomes.Sum(i => i.value);
                }
                await _appDbContext.SaveChangesAsync();
                response.data = wallet;
                response.code = true;
                response.message = "se actualizó el total de la cartera correctamente";
            }
            catch (Exception ex)
            {
                response.code = false;
                response.message = ex.Message;
            }
            return response;
        }
        
        /// <summary>
        /// Assigns users to wallets in the provided list.
        /// </summary>
        /// <param name="list">The array of wallets to assign users to.</param>
        /// <returns>An array of wallets with assigned users.</returns>
        public Wallet[] AsignUserToWallet(Wallet[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                var wallet = list[i];
                wallet.user = _utilsDal.GetUser(wallet.users_id);
                list[i] = wallet;
            }
            return list;
        }

        /// <summary>
        /// Creates a new wallet for a user.
        /// </summary>
        /// <param name="userid">The ID of the user for whom the wallet is created.</param>
        /// <param name="name">The name of the wallet.</param>
        /// <param name="icon">The icon of the wallet.</param>
        /// <returns>A response object containing information about the created wallet.</returns>
        public async Task<ResponseWallet> CreateWallet(int userid, string name, string icon)
        {
            ResponseWallet response = new ResponseWallet();
            try
            {

                response = await _utilsDal.CreateWallet(userid, name, icon);
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
        /// Deletes a wallet and transfers its total value to a new wallet.
        /// </summary>
        /// <param name="walletid">The ID of the wallet to delete.</param>
        /// <param name="newWallet">The ID of the new wallet to transfer the total value to.</param>
        /// <returns>A response object containing information about the deletion and transfer.</returns>
        public async Task<ResponseWallet> DeleteWallet(int walletid, int newWallet)
        {
            ResponseWallet response = new ResponseWallet();
            try
            {
                var list = _utilsDal.GetExpensesByCategory(walletid);
                var wallet = _appDbContext.wallets.FirstOrDefault(i => i.wallet_id == walletid);
                wallet.user = _utilsDal.GetUser(wallet.users_id);
                wallet.status = "inactive";
                var total = wallet.total;
                await _appDbContext.SaveChangesAsync();
                response.code = true;
                response.message = "Se eliminó correctamente la cartera, ";
                var res2 = await ChangeWalletToWallet(total, newWallet);
                response.message += res2.message;
                response.wallet = res2.wallet;
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
        /// Transfers the total value from one wallet to another.
        /// </summary>
        /// <param name="total">The total value to transfer.</param>
        /// <param name="newWallet">The ID of the new wallet to transfer the total value to.</param>
        /// <returns>A response object containing information about the transfer.</returns>
        private async Task<ResponseWallet> ChangeWalletToWallet(double total, int newWallet)
        {
            ResponseWallet res = new ResponseWallet();
            try
            {

                if (total == 0)
                {
                    res.message = "La cartera no cuenta dinero";
                    res.code = false;
                    return res;
                }
                var wallet = _appDbContext.wallets.FirstOrDefault(w => w.wallet_id == newWallet);
                wallet.total = total;
                await _appDbContext.SaveChangesAsync();
                res.message = "Se actualizó el valor de la cartera correctamente";
                res.code = true;
                res.wallet = Utils.GetWalletDto(wallet);
                return res;
            }
            catch (Exception ex)
            {
                res.message = "Hubo un error cambiando el valor de la cartera " + ex.Message;
                res.code = false;
                return res;
            }
        }

        /// <summary>
        /// Modifies a wallet with the specified details.
        /// </summary>
        /// <param name="wallet">The DTO representing the wallet to modify.</param>
        /// <param name="total">The new total value of the wallet.</param>
        /// <param name="icon">The new icon of the wallet.</param>
        /// <param name="name">The new name of the wallet.</param>
        /// <returns>A response object containing information about the modification.</returns>
        public async Task<ResponseWallet> ModifyWallet(WalletDto wallet, double total, string icon, string name)
        {
            ResponseWallet response = new ResponseWallet();
            try
            {
                var newWallet = _appDbContext.wallets.FirstOrDefault(i => i.wallet_id == wallet.wallet_id);
                var budget = _appDbContext.budget.FirstOrDefault(b => b.users_id == newWallet.users_id);
                newWallet.name = name;
                if (newWallet.total > total)
                {
                    var val = newWallet.total - total;
                    budget.value -= val;
                }
                if (newWallet.total < total)
                {
                    var val = total - newWallet.total;
                    budget.value += val;
                }
                newWallet.total = total;
                newWallet.icon = icon;
                await _appDbContext.SaveChangesAsync();
                response.code = true;
                response.message = "Se modificó correctamente la cartera";
                response.wallet = Utils.GetWalletDto(newWallet);
            }
            catch (Exception ex)
            {
                response.code = false;
                response.message = ex.Message;
                return response;
            }

            return response;
        }
    }
}
