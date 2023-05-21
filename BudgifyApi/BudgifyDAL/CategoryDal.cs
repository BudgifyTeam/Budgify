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
