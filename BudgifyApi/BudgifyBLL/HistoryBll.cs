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
    /// Represents the business logic layer for managing history-related operations.
    /// </summary>
    public class HistoryBll
    {
        private readonly UtilsDal _utilsDal;
        private readonly HistoryDal _HistoryDal;

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryBll"/> class with the specified <see cref="AppDbContext"/> object.
        /// </summary>
        /// <param name="db">The database context.</param>
        public HistoryBll(AppDbContext db)
        {
            _utilsDal = new UtilsDal(db);
            _HistoryDal = new HistoryDal(db, _utilsDal);
        }

        /// <summary>
        /// Retrieves the history of expenses and incomes for a specified user within a given date range.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="date">The date to filter the history.</param>
        /// <param name="range">The range of the history (e.g., "day", "week", "month", "year").</param>
        /// <returns>A <see cref="ResponseHistory"/> object containing the history information.</returns>
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
