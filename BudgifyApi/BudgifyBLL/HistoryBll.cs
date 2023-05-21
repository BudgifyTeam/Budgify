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
    public class HistoryBll
    {
        private readonly UtilsDal _utilsDal;
        private readonly HistoryDal _HistoryDal;
        public HistoryBll(AppDbContext db)
        {
            _utilsDal = new UtilsDal(db);
            _HistoryDal = new HistoryDal(db, _utilsDal);
        }

        public ResponseList<HistoryDto> GetHistory(int userid, DateTime date, string range)
        {
            var response = new ResponseList<HistoryDto>();
            try
            {
                var list = _HistoryDal.GetHistory(userid, date, range);
                if (!list.Any())
                {
                    response.message = "El usuario no cuenta con categorias";
                    response.code = false;
                    return response;
                }
                //response.data = list.Select(Utils.GetHistoryDto).ToList();
                response.message = "categorias obtenidas exitosamente";
                response.code = true;
                if (!response.code)
                {
                    response.message = "Error al obtener las categorias";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.code = false;
            }
            return response;
        }
    }
}
