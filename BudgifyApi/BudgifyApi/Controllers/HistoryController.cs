using BudgifyBll;
using BudgifyDal;
using BudgifyModels;
using BudgifyModels.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BudgifyApi.Controllers
{
    /// <summary>
    /// API controller for managing History related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController: ControllerBase
    {
        private readonly HistoryBll _HistoryBll;
        private readonly ResponseError resError = new ResponseError();

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryController"/> class.
        /// </summary>
        /// <param name="db">The database context.</param>
        public HistoryController(AppDbContext db)
        {
            _HistoryBll = new HistoryBll(db);
        }

        /// <summary>
        /// Retrieves the history for a specific day.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="date">The date for the history (in string format).</param>
        /// <returns>The history information for the specified day.</returns>
        [HttpGet("GetDayHistory", Name = "GetDayHistory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<HistoryDto> GetDayHistory(int userid, string date)
        {
            ResponseHistory response = _HistoryBll.GetHistory(userid, Utils.convertDate(date), "day");

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Retrieves the history for a specific week.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="date">The date for the history (in string format).</param>
        /// <returns>The history information for the specified week.</returns>
        [HttpGet("GetWeekHistory", Name = "GetWeekHistory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<HistoryDto> GetWeekHistory(int userid, string date)
        {
            ResponseHistory response = _HistoryBll.GetHistory(userid, Utils.convertDate(date), "week");

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Retrieves the history for a specific month.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="date">The date for the history (in string format).</param>
        /// <returns>The history information for the specified month.</returns>
        [HttpGet("GetMonthHistory", Name = "GetMonthHistory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<HistoryDto> GetMonthHistory(int userid, string date)
        {
            ResponseHistory response = _HistoryBll.GetHistory(userid, Utils.convertDate(date), "month");

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Retrieves the history for a specific year.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="date">The date for the history (in string format).</param>
        /// <returns>The history information for the specified year.</returns>
        [HttpGet("GetYearHistory", Name = "GetYearHistory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<HistoryDto> GetYearHistory(int userid, string date)
        {
            ResponseHistory response = _HistoryBll.GetHistory(userid, Utils.convertDate(date), "year");

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
