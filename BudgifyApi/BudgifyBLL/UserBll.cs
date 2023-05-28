using BudgifyModels;
using BudgifyDal;
using BudgifyModels.Dto;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace BudgifyBll
{
    /// <summary>
    /// Represents the business logic layer for managing users.
    /// </summary>
    public class UserBll
    {

        private readonly UserDal _userDal;
        private readonly UtilsDal _utilsDal;
        private readonly ExpenseDal _expenseDal;
        private readonly IncomeDal _incomeDal;
        private readonly WalletDal _walletDal;
        private readonly PocketDal _pocketDal;
        private readonly BudgetDal _budgetDal;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserBll"/> class.
        /// </summary>
        /// <param name="db">The <see cref="AppDbContext"/> instance.</param>
        public UserBll(AppDbContext db)
        {
            _budgetDal = new BudgetDal(db, _utilsDal);
            _walletDal = new WalletDal(db, _utilsDal);
            _utilsDal = new UtilsDal(db);
            _pocketDal = new PocketDal(db, _utilsDal);
            _expenseDal = new ExpenseDal(db, _utilsDal, _budgetDal, _walletDal, _pocketDal);
            _incomeDal = new IncomeDal(db, _utilsDal, _budgetDal, _walletDal);
            _userDal = new UserDal(db, _utilsDal, _expenseDal, _incomeDal);
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">The user registration information.</param>
        /// <returns>A <see cref="Response{T}"/> object containing the registration result.</returns>
        public async Task<Response<user>> Register(UserRegister user)
        {
            Response<user> response = new Response<user>();
            try
            {
                var userToSave = new user
                {
                    email = user.Email,
                    username = user.Username,
                    token = user.Token,
                    status = true,
                    publicaccount = false,
                    icon = "https://firebasestorage.googleapis.com/v0/b/budgify-ed7a9.appspot.com/o/userimage.jpg?alt=media&token=df5dc86a-c48e-4786-9501-565b2ad15134"
                };

                if (_userDal.UsernameExist(userToSave.username))
                {
                    response.message = " Username already exists";
                    return response;
                }

                if (_userDal.EmailExist(userToSave.email))
                {
                    response.message = " Email already exists";
                    return response;
                }

                response = await _userDal.RegisterUser(userToSave);

                if (!response.code)
                {
                    response.message = " Error al registrar al usuario";
                }

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="user">The user login information.</param>
        /// <returns>A <see cref="Response{T}"/> object containing the login result.</returns>
        public Response<SessionDto> Login(UserLogin user)
        {
            try
            {
                var response = _userDal.Login(user);
                if (response.code)
                {
                    var sessionDto = GetSessionDto(response.data);
                    return new Response<SessionDto>
                    {
                        data = sessionDto,
                        message = response.message,
                        code = response.code
                    };
                }
                else
                {
                    response.message += " Error al validar al usuario";
                    return new Response<SessionDto>
                    {
                        message = response.message,
                        code = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new Response<SessionDto>
                {
                    message = ex.Message,
                    code = false
                };
            }
        }

        /// <summary>
        /// Converts a <see cref="Session"/> object to a <see cref="SessionDto"/> object.
        /// </summary>
        /// <param name="session">The session object to convert.</param>
        /// <returns>A <see cref="SessionDto"/> object.</returns>
        public SessionDto GetSessionDto(Session session)
        {
            return new SessionDto
            {
                UserId = session.UserId,
                User_icon = session.icon,
                Budget = session.Budget != null ? Utils.GetBudgetDto(session.Budget) : null,
                Categories = session.Categories?.Select(c => Utils.GetCategoryDto(c)).ToArray(),
                Expenses = session.Expenses?.Select(e => Utils.GetExpenseDto(e)).ToArray(),
                Incomes = session.Incomes?.Select(i => Utils.GetIncomeDto(i)).ToArray(),
                Pockets = session.Pockets?.Select(p => Utils.GetPocketDto(p)).ToArray(),
                Wallets = session.Wallets?.Select(w => Utils.GetWalletDto(w)).ToArray()
            };
        }

        /// <summary>
        /// Modifies a user's information.
        /// </summary>
        /// <param name="userid">The ID of the user to modify.</param>
        /// <param name="icon">The new icon for the user.</param>
        /// <param name="name">The new name for the user.</param>
        /// <param name="email">The new email for the user.</param>
        /// <param name="publicAccount">The new public account status for the user.</param>
        /// <param name="token">The new token for the user.</param>
        /// <returns>A <see cref="Response{T}"/> object containing the modified user's session information.</returns>
        public async Task<Response<SessionDto>> ModifyUser(int userid, string icon, string name, string email, bool publicAccount, string token)
        {
            Response<SessionDto> response = new Response<SessionDto>();
            try
            {
                var modifResponse = await _userDal.ModifyUser(userid, icon, name, email, publicAccount, token);
                if (modifResponse.code)
                {
                    var sessionDto = GetSessionDto(modifResponse.data);
                    return new Response<SessionDto>
                    {
                        data = sessionDto,
                        message = modifResponse.message,
                        code = modifResponse.code
                    };
                }
                if (!modifResponse.code)
                {
                    response.message += " Error al modificar el usuario" + modifResponse.message;
                }
            }
            catch (Exception ex)
            {
                response.message += ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="userid">The ID of the user to delete.</param>
        /// <returns>A <see cref="Response{T}"/> object containing the deletion result.</returns>
        public async Task<Response<string>> DeleteUser(int userid)
        {
            Response<string> response = new Response<string>();
            try
            {
                response = await _userDal.DeleteUser(userid);
                if (!response.code)
                {
                    response.message += " Error al eliminar el usuario";
                }
            }
            catch (Exception ex)
            {
                response.message += ex.Message;
            }
            return response;
        }
    }
}
