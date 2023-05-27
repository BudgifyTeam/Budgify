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

    /// <summary>
    /// The business logic layer for managing wallets. It communicates with the data layer.
    /// </summary>
    public class WalletBll
    {
        private readonly UtilsDal _utilsDal;
        private readonly WalletDal _walletDal;

        /// <summary>
        /// Initializes a new instance of the WalletBll class with the specified database context.
        /// </summary>
        /// <param name="db">The AppDbContext object representing the database context.</param>
        public WalletBll(AppDbContext db)
        {
            _utilsDal = new UtilsDal(db);
            _walletDal = new WalletDal(db, _utilsDal);
        }

        /// <summary>
        /// Creates a new wallet for a user.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="name">The name of the wallet.</param>
        /// <param name="icon">The icon of the wallet.</param>
        /// <returns>A ResponseWallet object containing the result of the creation operation.</returns>
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

        /// <summary>
        /// Deletes a wallet and assigns its associated expenses to a new wallet.
        /// </summary>
        /// <param name="walletid">The ID of the wallet to delete.</param>
        /// <param name="newWallet">The ID of the new wallet to assign the expenses.</param>
        /// <returns>A ResponseWallet object containing the result of the deletion operation.</returns>
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

        /// <summary>
        /// Retrieves the wallets associated with a user.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <returns>A ResponseList of WalletDto objects containing the wallets associated with the user.</returns>
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


        /// <summary>
        /// Modifies a wallet by updating its properties with the provided values.
        /// </summary>
        /// <param name="wallet">The WalletDto object representing the wallet to modify.</param>
        /// <param name="total">The new total value of the wallet.</param>
        /// <param name="icon">The new icon for the wallet.</param>
        /// <param name="name">The new name for the wallet.</param>
        /// <returns>A ResponseWallet object containing the result of the modification operation.</returns>
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
