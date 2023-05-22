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
    public class PocketBll
    {
        private readonly UtilsDal _utilsDal;
        private readonly PocketDal _pocketDal;
        public PocketBll(AppDbContext db)
        {
            _utilsDal = new UtilsDal(db);
            _pocketDal = new PocketDal(db, _utilsDal);
        }

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
