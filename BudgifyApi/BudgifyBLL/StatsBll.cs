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
    public class StatsBll
    {
        private readonly UtilsDal _utilsDal;
        private readonly StatsDal _statsDal;
        public StatsBll(AppDbContext db)
        {
            _utilsDal = new UtilsDal(db);
            _statsDal = new StatsDal(db, _utilsDal);
        }

        public ResponseCategoryStat GetExpensesByCategory(int userid, string range, DateTime date)
        {
            var response = new ResponseCategoryStat();
            try
            {
                response = _statsDal.GetExpenseByCategory(userid, date, range);
                if (!response.code)
                {
                    response.message += " Error al obtener las estadisticas";
                    response.code = false;
                    return response;
                }
                if (!response.stats.Any())
                {
                    response.message = $" El usuario no cuenta con categorias";
                    response.code = false;
                    return response;
                }
                response.message += " Se obtuvieron correctamente las estadisticas";
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
