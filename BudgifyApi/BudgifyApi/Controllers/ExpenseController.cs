using BudgifyBll;
using BudgifyDal;
using BudgifyModels;
using BudgifyModels.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BudgifyApi.Controllers
{
    /// <summary>
    /// API controller for managing expenses.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController: ControllerBase
    {
        private readonly ExpenseBll _expenseBll;
        private readonly ResponseError resError = new ResponseError();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseController"/> class.
        /// </summary>
        /// <param name="db">The database context.</param>
        public ExpenseController(AppDbContext db)
        {
            _expenseBll = new ExpenseBll(db);
        }

        /// <summary>
        /// Creates a new expense for a user.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="value">The value of the expense.</param>
        /// <param name="date">The date of the expense.</param>
        /// <param name="wallet_id">The ID of the wallet for the expense.</param>
        /// <param name="pocket_id">The ID of the pocket for the expense.</param>
        /// <param name="category_id">The ID of the category for the expense.</param>
        /// <returns>The response containing the created expense.</returns>
        [HttpPost("CreateExpense", Name = "CreateExpense")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseExpense>> CreateExpense(int userid, double value, string date, int wallet_id, int pocket_id, int category_id)
        {
            ResponseExpense response = await _expenseBll.CreateExpense(userid, value, Utils.convertDate(date), wallet_id, pocket_id, category_id);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Deletes an existing expense.
        /// </summary>
        /// <param name="expenseId">The ID of the expense to delete.</param>
        /// <returns>The response indicating the success of the operation.</returns>
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

        /// <summary>
        /// Retrieves a list of expenses for a user based on the specified range.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="range">The range of incomes to retrieve (day, week, month, year).</param>
        /// <returns>The response containing the list of incomes.</returns>
        [HttpGet("GetExpenses", Name = "GetExpenses")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ExpenseDto> GetExpenses(int userid, string range)
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

        /// <summary>
        /// Retrieves a list of expenses for a user based on the specified date.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="date">The date to retrieve expenses for.</param>
        /// <returns>The response containing the list of expenses for the specified date.</returns>
        [HttpGet("GetExpensesByDay", Name = "GetExpensesByDay")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ExpenseDto> GetExpensesByDay(int userid, string date)
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

        /// <summary>
        /// Modifies an existing income.
        /// </summary>
        /// <param name="expense">The updated expense information.</param>
        /// <param name="wallet_id">The ID of new the wallet for the expense.</param>
        /// <param name="pocket_id">The ID of new the pocket for the expense.</param>
        /// <param name="categoryid">The ID of new the category for the expense.</param>
        /// <returns>The response indicating the success of the modification.</returns>
        [HttpPut("ModifyExpense", Name = "ModifyExpense")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseExpense>> ModifyExpense([FromBody] ExpenseDto expense, int wallet_id, int pocket_id, int categoryid)
        {
            ResponseExpense response = await _expenseBll.ModifyExpense(expense, wallet_id, pocket_id, categoryid);

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
