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
    public class PocketDal
    {
        private readonly AppDbContext _appDbContext;
        private readonly UtilsDal _utilsDal;

        public PocketDal(AppDbContext db, UtilsDal utils)
        {
            _appDbContext = db;
            _utilsDal = utils;
        }

        /// <summary>
        /// Updates the total value of a pocket based on its associated expenses.
        /// </summary>
        /// <param name="pocketid">The ID of the pocket.</param>
        /// <returns>A response containing the updated pocket information.</returns>
        public async Task<Response<Pocket>> updatePocketValue(int pocketid)
        {
            Response<Pocket> response = new Response<Pocket>();
            try
            {
                var Pocket = _appDbContext.pockets.FirstOrDefault(b => b.pocket_id == pocketid);
                var expenses = _utilsDal.GetExpensesByPocket(pocketid);
                Pocket.total = Pocket.total - expenses.Sum(i => i.value);
                await _appDbContext.SaveChangesAsync();
                response.data = Pocket;
                response.code = true;
                response.message = "se actualizó el total de la cartera correctamente";
                return response;
            }
            catch (Exception ex)
            {
                response.code = false;
                response.message = "no fue posible actualizar el bolsillo, " + ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Assigns the user to each pocket in the provided list.
        /// </summary>
        /// <param name="list">The array of pockets.</param>
        /// <returns>The updated array of pockets with the user assigned.</returns>
        public Pocket[] AsignUserToPocket(Pocket[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                var Pocket = list[i];
                Pocket.user = _utilsDal.GetUser(Pocket.users_id);
                list[i] = Pocket;
            }
            return list;
        }

        /// <summary>
        /// Creates a new pocket for a user.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="name">The name of the pocket.</param>
        /// <param name="icon">The URL of the pocket's icon.</param>
        /// <param name="goal">The goal value for the pocket.</param>
        /// <returns>A response containing the created pocket information.</returns>
        public async Task<ResponsePocket> CreatePocket(int userid, string name, string icon, double goal)
        {
            ResponsePocket response = new ResponsePocket();
            try
            {
                response = await _utilsDal.CreatePocket(userid, name, goal, icon);
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
        /// Deletes a pocket and updates the total value of another pocket.
        /// </summary>
        /// <param name="pocketid">The ID of the pocket to be deleted.</param>
        /// <param name="newPocket">The ID of the pocket to receive the updated total value.</param>
        /// <returns>A response containing the result of the deletion and the updated pocket information.</returns>
        public async Task<ResponsePocket> DeletePocket(int pocketid, int newPocket)
        {
            ResponsePocket response = new ResponsePocket();
            try
            {
                var list = _utilsDal.GetExpensesByPocket(pocketid);
                var Pocket = _appDbContext.pockets.FirstOrDefault(i => i.pocket_id == pocketid);
                Pocket.user = _utilsDal.GetUser(Pocket.users_id);
                Pocket.status = "inactive";
                var total = Pocket.total;
                await _appDbContext.SaveChangesAsync();
                response.code = true;
                response.message = "Se eliminó correctamente el bolsillo, ";
                var res2 = await ChangePocketToPocket(total, newPocket);
                response.message += res2.message;
                response.pocket = res2.pocket;
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
        /// Changes the total value of a pocket.
        /// </summary>
        /// <param name="total">The new total value for the pocket.</param>
        /// <param name="newPocket">The ID of the pocket to be updated.</param>
        /// <returns>A response containing the updated pocket information.</returns>
        private async Task<ResponsePocket> ChangePocketToPocket(double total, int newPocket)
        {
            ResponsePocket res = new ResponsePocket();
            try
            {

                if (total == 0)
                {
                    res.message = "La bolsillo no cuenta dinero";
                    res.code = false;
                    return res;
                }
                var pocket = _appDbContext.pockets.FirstOrDefault(w => w.pocket_id == newPocket);
                pocket.total = total;
                await _appDbContext.SaveChangesAsync();
                res.message = "Se actualizó el valor de la cartera correctamente";
                res.code = true;
                res.pocket = Utils.GetPocketDto(pocket);
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
        /// Modifies the properties of a pocket.
        /// </summary>
        /// <param name="Pocket">The PocketDto object containing the pocket's information.</param>
        /// <param name="total">The new total value for the pocket.</param>
        /// <param name="icon">The new icon URL for the pocket.</param>
        /// <param name="name">The new name for the pocket.</param>
        /// <returns>A response containing the modified pocket information.</returns>
        public async Task<ResponsePocket> ModifyPocket(PocketDto Pocket, double total, string icon, string name)
        {
            ResponsePocket response = new ResponsePocket();
            try
            {
                var newPocket = _appDbContext.pockets.FirstOrDefault(i => i.pocket_id == Pocket.pocket_id);
                newPocket.name = name;
                newPocket.total = total;
                newPocket.icon = icon;
                await _appDbContext.SaveChangesAsync();
                response.code = true;
                response.message = "Se modificó correctamente la categoria";
                response.pocket = Utils.GetPocketDto(newPocket);
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
