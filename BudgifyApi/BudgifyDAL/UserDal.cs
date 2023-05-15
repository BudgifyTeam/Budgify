using BudgifyModels;
using BudgifyModels.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyDal
{
    public class UserDal
    {
        private readonly AppDbContext _appDbContext;
        private readonly FinancialDal _financialDal;

        public UserDal(AppDbContext db, FinancialDal fn)
        {
            _appDbContext = db;
            _financialDal = fn;
        }

        public async Task<Response<string>> Login(UserLogin user) {
            Response<string> response = new Response<string>();
            var username = user.Username;
            var token = user.Token;
            try {
                if (UserExist(username))
                {
                    if (validateToken(token, username))
                    {
                        response.code = true;
                        response.message = "login exitoso";
                        return response;
                    }
                    else
                    {
                        response.code = false;
                        response.message = "contraseña incorrecta";
                        return response;
                    }
                }
                else
                {
                    response.code = false;
                    response.message = "el usuario no existe";
                    return response;
                }
            } catch (Exception ex)
            {
                response.code = false;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Response<user>> RegisterUser(user user) {
            Response<user> response = new Response<user>();
            
            try {
                var latsid = GetLastUserId() + 1;
                user.users_id = latsid;
                _appDbContext.users.Add(user);
                await _appDbContext.SaveChangesAsync();
                response.message += await _financialDal.CreateBudget(latsid);
                response.message += await CreateDefaultCategories(latsid);
                response.message += await _financialDal.CreateWallet(latsid, "efectivo");
                response.message += await _financialDal.CreatePocket(latsid, "default", 0);
                response.message += " se añadió el registro exitosamente";
                response.code = true;
                response.data = _appDbContext.users.FirstOrDefault(u => u.users_id == user.users_id);
            }
            catch (Exception ex)
            {
                response.code = false;
                response.message = ex.Message;
                return response;
            }
            return response;
        }
        public async Task<string> CreateDefaultCategories(int userid)
        {
            var categories = new string[] { "comida", "ocio", "gastos fijos", "suscripciones" };
            var responses = await Task.WhenAll(categories.Select(cat => _financialDal.CreateCategory(userid, cat)));

            if (responses.All(res => res.code == 1))
            {
                return "Se crearon correctamente las categorias default.";
            }

            return $"Error al crear la categoría '{responses.First(res => res.code == 0).message}'.";
        }

        private int GetLastUserId()
        {
            return _appDbContext.users.ToList().OrderByDescending(u => u.users_id).FirstOrDefault().users_id;
        }

        public user GetUser(int id)
        {
            return _appDbContext.users.FirstOrDefault(u => u.users_id == id);
        }
        public bool EmailExist(string Email)
        {
            var user = _appDbContext.users.FirstOrDefault(u => u.email == Email);
            return !(user == null);
        }

        public bool UserExist(string username)
        {
            var user = _appDbContext.users.FirstOrDefault(u => u.username == username);
            return user != null;
        }

        public bool validateToken(string token, string username) {
            var user = _appDbContext.users.FirstOrDefault(u => u.username == username);
            return user.token == token;
        }
    }
}
