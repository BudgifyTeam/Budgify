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
    /// Represents the business logic layer for handling statistics-related operations.
    /// </summary>
    public class StatsBll
    {
        private readonly UtilsDal _utilsDal;
        private readonly StatsDal _statsDal;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatsBll"/> class.
        /// </summary>
        /// <param name="db">The instance of the application's database context.</param>
        public StatsBll(AppDbContext db)
        {
            _utilsDal = new UtilsDal(db);
            _statsDal = new StatsDal(db, _utilsDal);
        }

        /// <summary>
        /// Gets the expenses by category for a specified user and time range.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="range">The time range for the statistics.</param>
        /// <param name="date">The reference date for the statistics.</param>
        /// <returns>A <see cref="ResponseCategoryStat"/> object containing the category statistics.</returns>
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
