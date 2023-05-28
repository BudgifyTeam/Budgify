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
    /// Represents the business logic layer for handling pocket-related operations.
    /// </summary>
    public class PocketBll
    {
        private readonly UtilsDal _utilsDal;
        private readonly PocketDal _pocketDal;

        /// <summary>
        /// Initializes a new instance of the <see cref="PocketBll"/> class.
        /// </summary>
        /// <param name="db">The instance of the application's database context.</param>
        public PocketBll(AppDbContext db)
        {
            _utilsDal = new UtilsDal(db);
            _pocketDal = new PocketDal(db, _utilsDal);
        }

        /// <summary>
        /// Creates a new pocket for a specified user.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="name">The name of the pocket.</param>
        /// <param name="icon">The icon of the pocket.</param>
        /// <param name="goal">The goal amount of the pocket.</param>
        /// <returns>A <see cref="ResponsePocket"/> object containing the creation result.</returns>
        public async Task<ResponsePocket> CreatePocket(int userid, string name, string icon, double goal)
        {
            ResponsePocket response = new ResponsePocket();
            try
            {
                response = await _pocketDal.CreatePocket(userid, name, icon, goal);
                if (!response.code)
                {
                    response.message += " Error al registrar el bolsillo";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Deletes a pocket.
        /// </summary>
        /// <param name="pocketid">The ID of the pocket to delete.</param>
        /// <param name="newPocket">The ID of the new pocket to replace the deleted one.</param>
        /// <returns>A <see cref="ResponsePocket"/> object containing the deletion result.</returns>
        public async Task<ResponsePocket> DeletePocket(int pocketid, int newPocket)
        {
            ResponsePocket response = new ResponsePocket();
            try
            {
                response = await _pocketDal.DeletePocket(pocketid, newPocket);
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
        /// Gets the pockets for a specified user.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <returns>A <see cref="ResponseList{PocketDto}"/> object containing the user's pockets.</returns>
        public ResponseList<PocketDto> GetPockets(int userid)
        {
            var response = new ResponseList<PocketDto>();
            try
            {
                var list = _utilsDal.GetPocketsByUserId(userid);
                if (!list.Any())
                {
                    response.message += " El usuario no cuenta con bolsillos";
                    response.code = false;
                    return response;
                }
                list = _pocketDal.AsignUserToPocket(list);
                response.data = list.Select(Utils.GetPocketDto).ToList();
                response.message += " bolsillos obtenidos exitosamente";
                response.code = true;
                if (!response.code)
                {
                    response.message += " Error al obtener los bolsillos";
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
        /// Modifies a pocket.
        /// </summary>
        /// <param name="Pocket">The pocket to modify.</param>
        /// <param name="total">The new total amount of the pocket.</param>
        /// <param name="icon">The new icon of the pocket.</param>
        /// <param name="name">The new name of the pocket.</param>
        /// <returns>A <see cref="ResponsePocket"/> object containing the modification result.</returns>
        public async Task<ResponsePocket> ModifyPocket(PocketDto Pocket, double total, string icon , string name)
        {
            ResponsePocket response = new ResponsePocket();
            try
            {
                response = await _pocketDal.ModifyPocket(Pocket, total, icon, name);
                if (!response.code)
                {
                    response.message += " Error al modificar el bolsillo";
                }
            }
            catch (Exception ex)
            {
                response.message += ex.Message;
            }
            return response;
        }
    }
}
