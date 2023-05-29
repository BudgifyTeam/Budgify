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
        private readonly UtilsDal _utilsDal;
        private readonly ExpenseDal _expenseDal;
        private readonly IncomeDal _incomeDal;

        public UserDal(AppDbContext db, UtilsDal fn, ExpenseDal ed, IncomeDal id)
        {
            _appDbContext = db;
            _utilsDal = fn;
            _expenseDal = ed;
            _incomeDal = id;
        }

        /// <summary>
        /// Logs in the user with the provided login information.
        /// </summary>
        /// <param name="user">The user login information.</param>
        /// <returns>A response object containing the result of the login operation and the user's session information.</returns>
        public Response<Session> Login(UserLogin user) {
            Response<Session> response = new Response<Session>();
            var username = user.Username;
            var token = user.Token;
            try {
                if (UsernameExist(username))
                {
                    if (ValidateToken(token, username))
                    {
                        response.code = true;
                        response.message = "login exitoso";
                        response.data = GetSession(user);
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


        /// <summary>
        /// Retrieves the session information for the specified user.
        /// </summary>
        /// <param name="user">The user login information.</param>
        /// <returns>A session object containing the user's session information.</returns>
        public Session GetSession(UserLogin user)
        {
            var id = GetUserIdByUsername(user.Username);
            var expenseslist = _utilsDal.GetExpensesByUserId(id);
            expenseslist = _expenseDal.AsignPocketToExpenses(expenseslist);
            expenseslist = _expenseDal.AsignWalletToExpenses(expenseslist);
            expenseslist = _expenseDal.AsignCategoryToExpenses(expenseslist);
            var incomesList = _utilsDal.GetIncomesByUserId(id);
            incomesList = _incomeDal.AsignWalletToIncomes(incomesList);
            var session = new Session
            {
                UserId = id,
                Budget = _utilsDal.GetBudgetByUserId(id),
                Categories = _utilsDal.GetCategoriesByUserId(id),
                Expenses = expenseslist,
                Incomes = incomesList,
                Pockets = _utilsDal.GetPocketsByUserId(id),
                Wallets = _utilsDal.GetWalletsByUserId(id),
                icon = GetIconByUserId(id)
            };
            return session;
        }

        /// <summary>
        /// Retrieves the icon of the user with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The icon of the user.</returns>
        private string GetIconByUserId(int id)
        {
            return _appDbContext.users.FirstOrDefault(u => u.users_id == id).icon;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">The user object containing the user information.</param>
        /// <returns>A response object containing the result of the user registration.</returns>
        public async Task<Response<user>> RegisterUser(user user) {
            Response<user> response = new Response<user>();
            
            try {
                var latsid = GetLastUserId() + 1;
                user.users_id = latsid;
                _appDbContext.users.Add(user);
                await _appDbContext.SaveChangesAsync();
                response.message += "|" + await _utilsDal.CreateBudget(latsid);
                response.message += "|" + await CreateDefaultCategories(latsid);
                response.message += "|" + _utilsDal.CreateWallet(latsid, "efectivo", "https://firebasestorage.googleapis.com/v0/b/budgify-ed7a9.appspot.com/o/Wallets.png?alt=media&token=cca353ff-39e1-4d5e-a0ce-3f2cb93f977c").Result.message;
                response.message += "|" + _utilsDal.CreatePocket(latsid, "default", 0, "https://firebasestorage.googleapis.com/v0/b/budgify-ed7a9.appspot.com/o/Wallets.png?alt=media&token=cca353ff-39e1-4d5e-a0ce-3f2cb93f977c").Result.message;
                response.message += "|" + " se añadió el registro exitosamente";
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

        /// <summary>
        /// Creates default categories for the specified user.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <returns>A message indicating the result of the default category creation.</returns>
        public async Task<string> CreateDefaultCategories(int userid)
        {
            var categories = new string[] { "comida", "ajustes", "gastos fijos", "suscripciones" };
            ResponseCategory[] responses = new ResponseCategory[4];
            for (int i = 0; i < categories.Length; i++) {
                var res1 = await _utilsDal.CreateCategory(userid, categories[i]);
                responses[i] = res1;
            }

            if (responses.All(res => res.code))
            {
                return " Se crearon correctamente las categorias default.";
            }

            return $"Error al crear la categoría '{responses.First(res => res.code).message}'.";
        }

        /// <summary>
        /// Deletes the user with the specified ID.
        /// </summary>
        /// <param name="userid">The ID of the user to delete.</param>
        /// <returns>A response object containing the result of the user deletion.</returns>
        public async Task<Response<string>> DeleteUser(int userid)
        {
            Response<string> response = new Response<string>();
            try
            {
                var user = GetUser(userid);
                user.status = false;
                await _appDbContext.SaveChangesAsync();
                response.message = " Se eliminó el usuario exitosamente";
                response.code = true;
                return response;
            }
            catch (Exception e) {
                response.message += e.Message;
                response.code = false;
                return response;
            }
        }

        /// <summary>
        /// Modifies the user with the specified parameters.
        /// </summary>
        /// <param name="userid">The ID of the user to modify.</param>
        /// <param name="icon">The new icon for the user.</param>
        /// <param name="name">The new name for the user.</param>
        /// <param name="email">The new email for the user.</param>
        /// <param name="publicAccount">The new public account status for the user.</param>
        /// <param name="token">The new token for the user.</param>
        /// <returns>A response object containing the result of the user modification.</returns>
        public async Task<Response<Session>> ModifyUser(int userid, string icon, string name, string email, bool publicAccount, string token)
        {
            Response<Session> response = new Response<Session>();
            try
            {
                var oldUser = GetUser(userid);
                if (oldUser.username != name){ 
                    if (UsernameExist(name))
                    {
                        response.code = false;
                        response.message = " El nombre de usuario ya existe";
                        return response;
                    }
                }
                if (oldUser.email != email) {
                    if (EmailExist(email))
                    {
                        response.code = false;
                        response.message = " El email ya esta en uso por otro usuario";
                        return response;
                    }
                }
                oldUser.icon = icon;
                oldUser.username = name;
                oldUser.email = email;
                oldUser.token = token;
                oldUser.publicaccount = publicAccount;
                await _appDbContext.SaveChangesAsync();
                response.code = true;
                response.message = "Se modificó correctamente el usuario";
                var usertosession = new UserLogin
                {
                    Token = oldUser.token,
                    Username = oldUser.username
                };
                response.data = GetSession(usertosession);
                return response;
            }
            catch (Exception ex)
            {
                response.code = false;
                response.message = ex.Message;
                return response;
            }
        }

        /// <summary>
        /// Retrieves the last user ID in the database.
        /// </summary>
        /// <returns>The last user ID, or 0 if the database is empty or an error occurs.</returns>
        private int GetLastUserId()
        {
            try {
                return _appDbContext.users.ToList().OrderByDescending(u => u.users_id).FirstOrDefault().users_id;
            }
            catch 
            {
                return 0; 
            }
        }

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The user with the specified ID, or null if not found.</returns>
        public user GetUser(int id)
        {
            return _appDbContext.users.FirstOrDefault(u => u.users_id == id);
        }

        /// <summary>
        /// Checks if an email exists in the database.
        /// </summary>
        /// <param name="Email">The email to check.</param>
        /// <returns>True if the email exists, otherwise false.</returns>
        public bool EmailExist(string Email)
        {
            var user = _appDbContext.users.FirstOrDefault(u => u.email == Email);
            return !(user == null);
        }

        /// <summary>
        /// Checks if a username exists in the database.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>True if the username exists, otherwise false.</returns>
        public bool UsernameExist(string username)
        {
            var user = _appDbContext.users.FirstOrDefault(u => u.username == username && u.status == true);
            return user != null;
        }

        /// <summary>
        /// Validates a token for a given username.
        /// </summary>
        /// <param name="token">The token to validate.</param>
        /// <param name="username">The username to validate the token against.</param>
        /// <returns>True if the token is valid for the username, otherwise false.</returns>
        public bool ValidateToken(string token, string username)
        {
            var user = _appDbContext.users.FirstOrDefault(u => u.username == username);
            return user.token == token;
        }

        /// <summary>
        /// Retrieves the user ID based on a given username.
        /// </summary>
        /// <param name="username">The username to get the ID for.</param>
        /// <returns>The user ID for the given username, or 0 if not found.</returns>
        public int GetUserIdByUsername(string username)
        {
            return _appDbContext.users.FirstOrDefault(u => u.username == username)?.users_id ?? 0;
        }
    }
}
