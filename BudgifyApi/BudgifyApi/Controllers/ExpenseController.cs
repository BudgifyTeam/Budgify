using BudgifyBll;
using BudgifyDal;
using BudgifyModels;
using BudgifyModels.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BudgifyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController: ControllerBase
    {
        private readonly ExpenseBll _expenseBll;
        private readonly ResponseError resError = new ResponseError();

        public ExpenseController(AppDbContext db)
        {
            _expenseBll = new ExpenseBll(db);
        }

        [HttpPost("CreateExpense", Name = "CreateExpense")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseExpense>> CreateExpense(int userid, double value, string date, int wallet_id, int pocket_id)
        {
            ResponseExpense response = await _expenseBll.CreateExpense(userid, value, Utils.convertDate(date), wallet_id, pocket_id);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("DeleteExpense", Name = "DeleteExpense")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseExpense>> DeleteExpense(int expenseId)
        {
            ResponseExpense response = await _expenseBll.DeleteExpense(expenseId);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("GetExpenses", Name = "GetExpenses")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ExpenseDto>> GetExpenses(int userid, string range)//range{day, week, month, year}
        {
            ResponseList<ExpenseDto> response = _expenseBll.GetExpenses(userid, range);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }
        [HttpGet("GetExpensesByDay", Name = "GetExpensesByDay")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ExpenseDto>> GetExpensesByDay(int userid, string date)
        {
            ResponseList<ExpenseDto> response = _expenseBll.GetExpensesDay(userid, "day", Utils.convertDate(date));

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPut("ModifyExpense", Name = "ModifyExpense")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseExpense>> ModifyExpense([FromBody] IncomeDto income, int wallet_id, int pocket_id)
        {
            ResponseExpense response = await _expenseBll.ModifyExpense(income, wallet_id, pocket_id);

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
