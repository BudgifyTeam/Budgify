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
