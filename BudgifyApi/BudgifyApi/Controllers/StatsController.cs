using BudgifyBll;
using BudgifyDal;
using BudgifyModels;
using BudgifyModels.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BudgifyApi.Controllers
{
    /// <summary>
    /// Controller for managing statistics.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController: ControllerBase
    {
        private readonly StatsBll _statsBll;
        private readonly ResponseError resError = new ResponseError();

        public StatsController(AppDbContext db)
        {
            _statsBll = new StatsBll(db);
        }

        /// <summary>
        /// Retrieves expenses by category for a specific day.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="date">The date in the format of "yyyy-MM-ddTHH:mm:ssZ".</param>
        /// <returns>An ActionResult containing the response with expense statistics by category for the specified day.</returns>
        [HttpGet("GetExpensesByCategoryDay", Name = "GetExpensesByCategoryDay")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StatsCategory> GetExpensesByCategoryDay(int userid, string date)
        {
            ResponseCategoryStat response = _statsBll.GetExpensesByCategory(userid, "day", Utils.convertDate(date));

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Retrieves expenses by category for a specific week.
        /// </summary>
        /// <param name="categoryid">The ID of the category.</param>
        /// <param name="date">The date in the format of "yyyy-MM-ddTHH:mm:ssZ".</param>
        /// <returns>An ActionResult containing the response with expense statistics by category for the specified week.</returns>
        [HttpGet("GetExpensesByCategoryWeek", Name = "GetExpensesByCategoryWeek")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StatsCategory> GetExpensesByCategoryWeek(int categoryid, string date)
        {
            ResponseCategoryStat response = _statsBll.GetExpensesByCategory(categoryid, "week", Utils.convertDate(date));

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Retrieves expenses by category for a specific month.
        /// </summary>
        /// <param name="categoryid">The ID of the category.</param>
        /// <param name="date">The date in the format of "yyyy-MM-ddTHH:mm:ssZ".</param>
        /// <returns>An ActionResult containing the response with expense statistics by category for the specified month.</returns>
        [HttpGet("GetExpensesByCategoryMonth", Name = "GetExpensesByCategoryMonth")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StatsCategory> GetExpensesByCategoryMonth(int categoryid, string date)
        {
            ResponseCategoryStat response = _statsBll.GetExpensesByCategory(categoryid, "month", Utils.convertDate(date));

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Retrieves expenses by category for a specific year.
        /// </summary>
        /// <param name="categoryid">The ID of the category.</param>
        /// <param name="date">The date in the format of "yyyy-MM-ddTHH:mm:ssZ".</param>
        /// <returns>An ActionResult containing the response with expense statistics by category for the specified year.</returns>
        [HttpGet("GetExpensesByCategoryYear", Name = "GetExpensesByCategoryYear")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StatsCategory> GetExpensesByCategoryYear(int categoryid, string date)
        {
            ResponseCategoryStat response = _statsBll.GetExpensesByCategory(categoryid, "year", Utils.convertDate(date));

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
