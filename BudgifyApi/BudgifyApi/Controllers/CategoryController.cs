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
    public class CategoryController: ControllerBase
    {
        private readonly CategoryBll _categoryBll;
        private readonly ResponseError resError = new ResponseError();

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="db">The database context.</param>
        public CategoryController(AppDbContext db)
        {
            _categoryBll = new CategoryBll(db);
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="userid">The user ID.</param>
        /// <param name="name">The category name.</param>
        /// <returns>The response containing the created category.</returns>
        [HttpPost("CreateCategory", Name = "CreateCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseCategory>> CreateCategory(int userid, string name)
        {
            ResponseCategory response = await _categoryBll.CreateCategory(userid, name);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Deletes a category.
        /// </summary>
        /// <param name="categoryid">The ID of the category to delete.</param>
        /// <param name="newCategoryId">The ID of the category to replace the deleted one.</param>
        /// <returns>The response indicating the success of the deletion.</returns>
        [HttpGet("DeleteCategory", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseCategory>> DeleteCategory(int categoryid, int newCategoryId) 
        {
            ResponseCategory response = await _categoryBll.DeleteCategory(categoryid, newCategoryId);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Gets all categories for a user.
        /// </summary>
        /// <param name="userid">The user ID.</param>
        /// <returns>The response containing the list of categories.</returns>
        [HttpGet("GetCategories", Name = "GetCategories")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CategoryDto> GetCategories(int userid)//range{day, week, month, year}
        {
            ResponseList<CategoryDto> response = _categoryBll.GetCategories(userid);

            if (!response.code)
            {
                resError.message = response.message;
                resError.code = 0;

                return StatusCode(StatusCodes.Status400BadRequest, response);
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Modifies a category.
        /// </summary>
        /// <param name="category">The category data.</param>
        /// <returns>The response indicating the success of the modification.</returns>
        [HttpPut("ModifyCategory", Name = "ModifyCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseCategory>> ModifyCategory([FromBody] CategoryDto category)
        {
            ResponseCategory response = await _categoryBll.ModifyCategory(category);

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
