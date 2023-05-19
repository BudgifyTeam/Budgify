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

        public WalletDal(AppDbContext db, UtilsDal utils)
        {
            _appDbContext = db;
            _utilsDal = utils;
        }
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

        public async Task<ResponseWallet> ModifyWallet(WalletDto wallet, double total, string icon, string name)
        {
            ResponseWallet response = new ResponseWallet();
            try
            {
                var newWallet = _appDbContext.wallets.FirstOrDefault(i => i.wallet_id == wallet.wallet_id);
                newWallet.name = name;
                newWallet.total = total;
                newWallet.icon = icon;
                await _appDbContext.SaveChangesAsync();
                response.code = true;
                response.message = "Se modificó correctamente la categoria";
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
