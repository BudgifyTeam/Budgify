using BudgifyBll;
using BudgifyDal;
using BudgifyModels;
using BudgifyModels.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BudgifyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController: ControllerBase
    {
        private readonly HistoryBll _HistoryBll;
        private readonly ResponseError resError = new ResponseError();

        public HistoryController(AppDbContext db)
        {
            _HistoryBll = new HistoryBll(db);
        }

        [HttpGet("GetDayHistory", Name = "GetDayHistory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<HistoryDto> GetDayHistory(int userid, string date)
        {
            ResponseList<HistoryDto> response = _HistoryBll.GetHistory(userid, Utils.convertDate(date), "day");

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("GetWeekHistory", Name = "GetWeekHistory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<HistoryDto> GetWeekHistory(int userid, string date)
        {
            ResponseList<HistoryDto> response = _HistoryBll.GetHistory(userid, Utils.convertDate(date), "week");

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("GetMonthHistory", Name = "GetMonthHistory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<HistoryDto> GetMonthHistory(int userid, string date)
        {
            ResponseList<HistoryDto> response = _HistoryBll.GetHistory(userid, Utils.convertDate(date), "month");

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("GetYearHistory", Name = "GetYearHistory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<HistoryDto> GetYearHistory(int userid, string date)
        {
            ResponseList<HistoryDto> response = _HistoryBll.GetHistory(userid, Utils.convertDate(date), "year");

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
