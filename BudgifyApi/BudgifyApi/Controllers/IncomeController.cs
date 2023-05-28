using BudgifyBll;
using BudgifyDal;
using BudgifyModels;
using BudgifyModels.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace BudgifyApi.Controllers
{
    /// <summary>
    /// API controller for managing incomes.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IncomeBll _incomeBll;
        private readonly ResponseError resError = new ResponseError();

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeController"/> class.
        /// </summary>
        /// <param name="db">The database context.</param>
        public IncomeController(AppDbContext db)
        {
            _incomeBll = new IncomeBll(db);
        }

        /// <summary>
        /// Creates a new income for a user.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="value">The value of the income.</param>
        /// <param name="date">The date of the income.</param>
        /// <param name="wallet_id">The ID of the wallet for the income.</param>
        /// <returns>The response containing the created income.</returns>
        [HttpPost("CreateIncome", Name = "CreateIncome")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseIncome>> CreateIncome(int userid, double value, string date, int wallet_id)
        {
            ResponseIncome response = await _incomeBll.CreateIncome(userid, value, Utils.convertDate(date), wallet_id);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Deletes an existing income.
        /// </summary>
        /// <param name="incomeid">The ID of the income to delete.</param>
        /// <returns>The response indicating the success of the operation.</returns>
        [HttpGet("DeleteIncome", Name = "DeleteIncome")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseIncome>> DeleteIncome(int incomeid)
        {
            ResponseIncome response = await _incomeBll.DeleteIncome(incomeid);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Retrieves a list of incomes for a user based on the specified range.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="range">The range of incomes to retrieve (day, week, month, year).</param>
        /// <returns>The response containing the list of incomes.</returns>
        [HttpGet("GetIncomes", Name = "GetIncomes")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IncomeDto> GetIncomes(int userid, string range)
        {
            ResponseList<IncomeDto> response = _incomeBll.GetIncomes(userid, range);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Retrieves a list of incomes for a user based on the specified date.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="date">The date to retrieve incomes for.</param>
        /// <returns>The response containing the list of incomes for the specified date.</returns>
        [HttpGet("GetIncomesByDay", Name = "GetIncomesByDay")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IncomeDto> GetIncomesByDay(int userid, string date)
        {
            ResponseList<IncomeDto> response = _incomeBll.GetIncomesDay(userid, "day", Utils.convertDate(date));

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Modifies an existing income.
        /// </summary>
        /// <param name="income">The updated income information.</param>
        /// <param name="wallet_id">The ID of the wallet for the income.</param>
        /// <returns>The response indicating the success of the modification.</returns>
        [HttpPut("ModifyIncome", Name = "ModifyIncome")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseIncome>> ModifyIncome([FromBody] IncomeDto income, int wallet_id)
        {
            ResponseIncome response = await _incomeBll.ModifyIncome(income, wallet_id);

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
