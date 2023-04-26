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
        public async Task<Response<user>> Register(UserRegister user) { 
            Response<user> response = new Response<user>();
            try {
                user userToSave = new user {
                    Email = user.Email,
                    Username = user.Username,
                    Token = user.Token,
                    Status = true,
                    PublicAccount = false,
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
