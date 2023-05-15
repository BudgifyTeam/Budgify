using BudgifyModels;
using BudgifyModels.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyDal
{
    public class UserDal
    {
        private readonly AppDbContext _appDbContext;

        public UserDal(AppDbContext db)
        {
            _appDbContext = db;
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
                var verifyUser = UserExist(user.username);
                var verifyEmail = EmailExist(user.email);
                user.users_id = GetLastId() + 1;
                _appDbContext.users.Add(user);
                await _appDbContext.SaveChangesAsync();
                response.message = "se añadió el registro exitosamente";
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

        private int GetLastId()
        {
            return _appDbContext.users.ToList().OrderByDescending(u => u.users_id).FirstOrDefault().users_id;
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
