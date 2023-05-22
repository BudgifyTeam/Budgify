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

        public ResponseHistory GetHistory(int userid, DateTime date, string range)
        {
            var response = new ResponseHistory();
            try
            {
                response = _HistoryDal.GetHistory(userid, date, range);
                if (!response.code)
                {
                    response.message += " Error al obtener el historial";
                }
                if (!response.history.Items.Any())
                {
                    response.message = $"El usuario no cuenta con gastos o ingresos para el rango: {range}";
                    response.code = false;
                    return response;
                }
                response.message = "Se obtuvo el historial correctamente";
                response.code = true;
            }
            catch (Exception ex)
            {
                response.message += ex.Message;
                response.code = false;
            }
            return response;
        }
    }
}
