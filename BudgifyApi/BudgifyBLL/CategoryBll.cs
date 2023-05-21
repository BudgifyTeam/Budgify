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
    public class CategoryBll
    {
        private readonly UtilsDal _utilsDal;
        private readonly CategoryDal _categoryDal;
        public CategoryBll(AppDbContext db)
        {
            _utilsDal = new UtilsDal(db);
            _categoryDal = new CategoryDal(db, _utilsDal);
        }
      
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
