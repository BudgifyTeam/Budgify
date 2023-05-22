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
    public class WalletBll
    {
        private readonly UtilsDal _utilsDal;
        private readonly WalletDal _walletDal;
        public WalletBll(AppDbContext db)
        {
            _utilsDal = new UtilsDal(db);
            _walletDal = new WalletDal(db, _utilsDal);
        }

        public async Task<ResponseWallet> CreateWallet(int userid, string name, string icon)
        {
            ResponseWallet response = new ResponseWallet();
            try
            {
                response = await _walletDal.CreateWallet(userid, name, icon);
                if (!response.code)
                {
                    response.message = "Error al registrar la cartera";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseWallet> DeleteWallet(int walletid, int newWallet)
        {
            ResponseWallet response = new ResponseWallet();
            try
            {
                response = await _walletDal.DeleteWallet(walletid, newWallet);
                if (!response.code)
                {
                    response.message = "Error al eliminar la cartera";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }

        public ResponseList<WalletDto> GetWallets(int userid)
        {
            var response = new ResponseList<WalletDto>();
            try
            {
                var list = _utilsDal.GetWalletsByUserId(userid);
                if (!list.Any())
                {
                    response.message = "El usuario no cuenta con carteras";
                    response.code = false;
                    return response;
                }
                list = _walletDal.AsignUserToWallet(list);
                response.data = list.Select(Utils.GetWalletDto).ToList();
                response.message = "carteras obtenidas exitosamente";
                response.code = true;
                if (!response.code)
                {
                    response.message = "Error al obtener las carteras";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.code = false;
            }
            return response;
        }

        public async Task<ResponseWallet> ModifyWallet(WalletDto wallet, double total, string icon , string name)
        {
            ResponseWallet response = new ResponseWallet();
            try
            {
                response = await _walletDal.ModifyWallet(wallet, total, icon, name);
                if (!response.code)
                {
                    response.message += "Error al modificar la cartera";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }
    }
}
