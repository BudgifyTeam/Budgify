using BudgifyModels;
using BudgifyModels.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyDal
{
    public class CategoryDal
    {
        private readonly AppDbContext _appDbContext;
        private readonly UtilsDal _utilsDal;

        public CategoryDal(AppDbContext db, UtilsDal fn)
        {
            _appDbContext = db;
            _utilsDal = fn;
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="userid">The ID of the user associated with the category.</param>
        /// <param name="name">The name of the category.</param>
        /// <returns>A ResponseCategory object containing the status and created category.</returns>
        public async Task<ResponseCategory> CreateCategory(int userid, string name)
       {
           ResponseCategory response = new ResponseCategory();
           try
           {
               response = await _utilsDal.CreateCategory(userid, name);
           }
           catch (Exception ex)
           {
               response.code = false;
               response.message = ex.Message;
               return response;
           }
           return response;
       }

        /// <summary>
        /// Changes the category of expenses.
        /// </summary>
        /// <param name="list">The array of expenses to be updated.</param>
        /// <param name="categoryid">The ID of the new category.</param>
        /// <returns>A ResponseList object containing the status and updated expenses.</returns>
        public async Task<ResponseList<Expense>> changeExpenseCategory(Expense[] list, int categoryid)
        {
            ResponseList<Expense> res = new ResponseList<Expense>();
            try {
                
                if (!list.Any())
                {
                    res.message = "La Categoria no cuenta con gastos asociados";
                    res.code = false;
                    return res;
                }
                for (int i = 0; i < list.Length; i++)
                {
                    var expense = list[i];
                    expense.category = _utilsDal.GetCategory(categoryid);
                    list[i] = expense;
                }
                await _appDbContext.SaveChangesAsync();
                res.message = "Se actualizaron las categorias de los gastos correctamente";
                res.code = true;
                res.data = list.ToList();
                return res;
            }
            catch (Exception ex)
            {
                res.message = "Hubo un error cambiando los gastos de categoria " + ex.Message;
                res.code = false;
                return res;
            }
        }

        /// <summary>
        /// Modifies the details of a category.
        /// </summary>
        /// <param name="newCategory">The updated details of the category.</param>
        /// <returns>A ResponseCategory object indicating the status of the operation.</returns>
        public async Task<ResponseCategory> ModifyCategory(CategoryDto newCategory)
        {
            ResponseCategory response = new ResponseCategory();
            try
            {
                var category = _appDbContext.categories.FirstOrDefault(i => i.category_id == newCategory.category_id);
                category.name = newCategory.name;
                await _appDbContext.SaveChangesAsync();
                response.code = true;
                response.message = "Se modificó correctamente la categoria";
                response.category = Utils.GetCategoryDto(category);
            }
            catch (Exception ex)
            {
                response.code = false;
                response.message = ex.Message;
                return response;
            }

            return response;
        }

        /// <summary>
        /// Deletes a category and updates the expenses associated with it to a new category.
        /// </summary>
        /// <param name="categoryid">The ID of the category to be deleted.</param>
        /// <param name="newCategory">The ID of the new category to assign the expenses to.</param>
        /// <returns>A ResponseCategory object indicating the status of the operation.</returns>
        public async Task<ResponseCategory> DeleteCategory(int categoryid, int newCategory)
        {
            ResponseCategory response = new ResponseCategory();
            try
            {
                var list = _utilsDal.GetExpensesByCategory(categoryid);
                var category = _appDbContext.categories.FirstOrDefault(i => i.category_id == categoryid);
                category.user = _utilsDal.GetUser(category.users_id);
                category.status = "inactive";
                await _appDbContext.SaveChangesAsync();
                response.code = true;
                response.message = "Se eliminó correctamente la categoria, ";
                response.message += changeExpenseCategory(list, newCategory).Result.message;
                response.category = Utils.GetCategoryDto(category);
            }
            catch (Exception ex)
            {
                response.code = false;
                response.message = ex.Message;
                return response;
            }

            return response;
        }

        /// <summary>
        /// Assigns the user information to each category in the provided list.
        /// </summary>
        /// <param name="list">The array of Category objects.</param>
        /// <returns>An array of Category objects with user information assigned.</returns>
        public Category[] AsignUserToCategories(Category[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                var category = list[i];
                category.user = _utilsDal.GetUser(category.users_id);
                list[i] = category;
            }
            return list;
        }
    }
}
