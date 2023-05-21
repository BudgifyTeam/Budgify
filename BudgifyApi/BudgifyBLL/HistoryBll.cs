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

        public async Task<ResponseHistory> GetHistory(int userid, DateTime date, string range)
        {
            var response = new ResponseHistory();
            try
            {
                response.history = await _HistoryDal.GetHistory(userid, date, range);
                if (!response.history.Incomes.Any() && !response.history.Expenses.Any())
                {
                    response.message = $"El usuario no cuenta con gastos o ingresos para el rango: {range}";
                    response.code = false;
                    return response;
                }
                response.message = "Se obtuvo el historial correctamente";
                response.code = true;
                if (!response.code)
                {
                    response.message = "Error al obtener el historial";
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
