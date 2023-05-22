using BudgifyBll;
using BudgifyDal;
using BudgifyModels;
using BudgifyModels.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BudgifyApi.Controllers
{
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
