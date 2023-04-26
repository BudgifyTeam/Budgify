using BudgifyModels;
using BudgifyDal;
using BudgifyModels.Dto;

namespace BudgifyBll
{
    public class UserBll
    {

        private readonly UserDal _userDal;
        public UserBll(AppDbContext db)
        {
            _userDal = new UserDal(db);
        }
        public async Task<Response<User>> Register(UserDto user) { 
            Response<User> response = new Response<User>();
            try {
                User userToSave = new User { 
                    Id = 0,
                    Email = user.Email,
                    Username = user.Username,
                    Token = user.Token,
                };
                response = await _userDal.RegisterUser(userToSave);
                if (response.code)
                {
                    return response;
                }
                else {
                    response.message = "Error al registrar al usuario";
                    response.code = false;
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.code = false;
            }
            return response;
        }
    }
}
