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

        public async Task<Response<user>> RegisterUser(user user) {
            Response<user> response = new Response<user>();
            var verifyUser = verifyUsers(user.Username);
            var verifyEmail = VerifyEmail(user.Email);
            try {
                user.Id = GetLastId() + 1;
                _appDbContext.users.Add(user);
                await _appDbContext.SaveChangesAsync();
                response.message = "se añadió el registro exitosamente";
                response.code = true;
                response.data = _appDbContext.users.FirstOrDefault(u => u.Id == user.Id);
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
            return _appDbContext.users.ToList().OrderByDescending(u => u.Id).FirstOrDefault().Id;
        }

        public Boolean VerifyEmail(string email)
        {
            try
            {
                var verify = _appDbContext.users.FirstOrDefault(users => users.Email == email);
                if(verify == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }catch (Exception ex)
            {
                return false;
            }
        }
        
        public Boolean verifyUsers(string username)
        {
            try
            {
                var users = _appDbContext.users.FirstOrDefault(users => users.Username == username);
                if (users == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }catch(Exception ex)
            {
                return false;
            }
        }
    }
}
