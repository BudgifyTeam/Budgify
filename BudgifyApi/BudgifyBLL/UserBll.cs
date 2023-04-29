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
        public async Task<Response<user>> Register(UserRegister user)
        {
            Response<user> response = new Response<user>();
            try
            {
                var userToSave = new user
                {
                    Email = user.Email,
                    Username = user.Username,
                    Token = user.Token,
                    Status = true,
                    PublicAccount = false,
                };

                if (_userDal.UserExist(userToSave.Username))
                {
                    response.message = "username already exists";
                    return response;
                }

                if (_userDal.EmailExist(userToSave.Email))
                {
                    response.message = "Email already exists";
                    return response;
                }

                response = await _userDal.RegisterUser(userToSave);

                if (!response.code)
                {
                    response.message = "Error al registrar al usuario";
                }

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }

            return response;
        }
        public async Task<Response<string>> Login(UserLogin user)
        {
            Response<string> response = new Response<string>();
            try
            {
                response = await _userDal.Login(user);
                if (response.code)
                {
                    return response;
                }
                else
                {
                    response.message = "Error al validar al usuario";
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
