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
                var verifyUsername = _userDal.verifyUsers(userToSave.Username);
                var verifyEmail = _userDal.VerifyEmail(userToSave.Email);
                if(verifyUsername == true)
                {
                    if(verifyEmail == true)
                    {
                        response = await _userDal.RegisterUser(userToSave);
                        if (response.code)
                        {
                            return response;
                        }
                        else
                        {
                            response.message = "Error al registrar al usuario";
                            response.code = false;
                        }
                    }
                    else
                    {
                        response.message = "Email already exist";
                        response.code = false;
                    }
                
                }
                else
                {
                    response.message = "username already exist";
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
