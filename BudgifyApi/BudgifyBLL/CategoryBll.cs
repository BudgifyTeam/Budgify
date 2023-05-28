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
    /// Represents the business logic layer for managing categories.
    /// </summary>
    public class CategoryBll
    {
        private readonly UtilsDal _utilsDal;
        private readonly CategoryDal _categoryDal;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryBll"/> class.
        /// </summary>
        /// <param name="db">The <see cref="AppDbContext"/> instance for database operations.</param>
        public CategoryBll(AppDbContext db)
        {
            _utilsDal = new UtilsDal(db);
            _categoryDal = new CategoryDal(db, _utilsDal);
        }

        /// <summary>
        /// Creates a new category for the specified user.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="name">The name of the category.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains the response for creating the category.</returns>
        public async Task<ResponseCategory> CreateCategory(int userid, string name)
        {
            ResponseCategory response = new ResponseCategory();
            try
            {
                response = await _categoryDal.CreateCategory(userid, name);
                if (!response.code)
                {
                    response.message = "Error al crear la categoria";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Deletes the specified category and updates the expenses associated with it to a new category.
        /// </summary>
        /// <param name="categoryid">The ID of the category to delete.</param>
        /// <param name="newCategoryId">The ID of the new category to assign the expenses to.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains the response for deleting the category.</returns>
        public async Task<ResponseCategory> DeleteCategory(int categoryid, int newCategoryId)
        {
            ResponseCategory response = new ResponseCategory();
            try
            {
                response = await _categoryDal.DeleteCategory(categoryid, newCategoryId);
                if (!response.code)
                {
                    response.message = "Error al eliminar la categoria";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Retrieves the list of categories for the specified user.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <returns>A <see cref="ResponseList{T}"/> containing the list of categories for the user.</returns>
        public ResponseList<CategoryDto> GetCategories(int userid)
        {
            var response = new ResponseList<CategoryDto>();
            try
            {
                var list = _utilsDal.GetCategoriesByUserId(userid);
                if (!list.Any())
                {
                    response.message = "El usuario no cuenta con categorias";
                    response.code = false;
                    return response;
                }
                list = _categoryDal.AsignUserToCategories(list);
                response.data = list.Select(Utils.GetCategoryDto).ToList();
                response.message = "categorias obtenidas exitosamente";
                response.code = true;
                if (!response.code)
                {
                    response.message = "Error al obtener las categorias";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.code = false;
            }
            return response;
        }

        /// <summary>
        /// Modifies the specified category.
        /// </summary>
        /// <param name="category">The category to modify.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains the response for modifying the category.</returns>
        public async Task<ResponseCategory> ModifyCategory(CategoryDto category)
        {
            ResponseCategory response = new ResponseCategory();
            try
            {
                response = await _categoryDal.ModifyCategory(category);
                if (!response.code)
                {
                    response.message = "Error al modificar la categoria";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }
    }
}
