using BudgifyModels;
using BudgifyDal;
using BudgifyModels.Dto;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace BudgifyBll
{
    public class UserBll
    {

        private readonly UserDal _userDal;
        private readonly UtilsDal _utilsDal;
        private readonly ExpenseDal _expenseDal;
        private readonly IncomeDal _incomeDal;
        private readonly WalletDal _walletDal;
        private readonly PocketDal _pocketDal;
        private readonly BudgetDal _budgetDal;
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

                if (_userDal.UserExist(userToSave.username))
                {
                    response.message = "username already exists";
                    return response;
                }

                if (_userDal.EmailExist(userToSave.email))
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
    }
}
