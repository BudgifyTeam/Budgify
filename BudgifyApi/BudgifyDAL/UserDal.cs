using BudgifyModels;
using BudgifyModels.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        public async Task<Response<User>> RegisterUser(User user) {
            Response<User> response = new Response<User>();
            try {
                user.Id = GetLastId() + 1;
                _appDbContext.users.Add(user);
                await _appDbContext.SaveChangesAsync();
                response.message = "se añadió el registro exitosamente";
                response.code = true;
                response.data = _appDbContext.users.FirstOrDefault();
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
    }
}
