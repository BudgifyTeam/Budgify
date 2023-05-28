using BudgifyDal;
using BudgifyModels;
using BudgifyModels.Dto;
using Npgsql.Replication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace BudgifyBll
{
    /// <summary>
    /// Represents the business logic layer for handling income-related operations.
    /// </summary>
    public class IncomeBll
    {
        private readonly UtilsDal _utilsDal;
        private readonly IncomeDal _incomeDal;
        private readonly BudgetDal _budgifyDal;
        private readonly WalletDal _walletDal;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeBll"/> class.
        /// </summary>
        /// <param name="db">The instance of the application's database context.</param>
        public IncomeBll(AppDbContext db)
        {
           
            _utilsDal = new UtilsDal(db);
            _budgifyDal = new BudgetDal(db, _utilsDal);
            _walletDal = new WalletDal(db, _utilsDal);
            _incomeDal = new IncomeDal(db, _utilsDal, _budgifyDal, _walletDal);
        }

        /// <summary>
        /// Creates a new income for a specified user and wallet.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="value">The value of the income.</param>
        /// <param name="date">The date of the income.</param>
        /// <param name="wallet_id">The ID of the wallet associated with the income.</param>
        /// <returns>A <see cref="ResponseIncome"/> object containing the creation result.</returns>
        public async Task<ResponseIncome> CreateIncome(int userid, double value, DateTime date, int wallet_id)
        {
            ResponseIncome response = new ResponseIncome();
            try {
                var newIncome = new Income()
                {
                    date = date,
                    income_id = 0,
                    status = "active",
                    users_id = userid,
                    value = value
                };
                response = await _incomeDal.CreateIncome(newIncome, wallet_id);
                if (!response.code)
                {
                    response.message = "Error al registrar al ingreso";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Deletes an income.
        /// </summary>
        /// <param name="incomeid">The ID of the income to delete.</param>
        /// <returns>A <see cref="ResponseIncome"/> object containing the deletion result.</returns>
        public async Task<ResponseIncome> DeleteIncome(int incomeid)
        {
            ResponseIncome response = new ResponseIncome();
            try
            {
                response = await _incomeDal.DeleteIncome(incomeid);
                if (!response.code)
                {
                    response.message = "Error al eliminar al ingreso";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Gets the incomes for a specified user within a given range.
        /// </summary>
        /// <param name="userid">The ID of the user.</param>
        /// <param name="range">The range of incomes to retrieve (day, week, month, year).</param>
        /// <returns>A <see cref="ResponseList{IncomeDto}"/> object containing the user's incomes within the specified range.</returns>
        public ResponseList<IncomeDto> GetIncomes(int userid, string range)
        {
            var response = new ResponseList<IncomeDto>();
            try
            {
                var list = _utilsDal.GetIncomesByUserId(userid);
                if (!list.Any())
                {
                    response.message = "El usuario no cuenta con ingresos";
                    response.code = false;
                    return response;
                }
                list = _incomeDal.AsignWalletToIncomes(list);
                var incomeList = list.Select(Utils.GetIncomeDto).ToList();
                response.data = GetIncomesByRange(incomeList, range, DateTime.Today).ToList();
                response.message = "Ingresos obtenidos exitosamente";
                response.code = true;
                if (!response.code)
                {
                    response.message = "Error al obtener los ingresos";
                }
            }
            catch (Exception ex)
            { 
                response.message = ex.Message;
                response.code = false;
            }
            return response;
        }

        /// <summary>
        /// Gets the incomes for a specified user within a given range and date.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="range">The range of incomes to retrieve (day, week, month, year).</param>
        /// <param name="date">The specific date within the range.</param>
        /// <returns>A <see cref="ResponseList{IncomeDto}"/> object containing the user's incomes within the specified range and date.</returns>
        public ResponseList<IncomeDto> GetIncomesDay(int userId, string range, DateTime date)
        {
            var response = new ResponseList<IncomeDto>();
            try
            {
                var list = _utilsDal.GetIncomesByUserId(userId);
                if (!list.Any())
                {
                    response.message = "El usuario no cuenta con ingresos";
                    response.code = false;
                    return response;
                }
                list = _incomeDal.AsignWalletToIncomes(list);
                var incomeList = list.Select(Utils.GetIncomeDto).ToList();
                response.data = GetIncomesByRange(incomeList, range, date).ToList();
                response.message = "Ingresos obtenidos exitosamente";
                response.code = true;
                if (!response.code)
                {
                    response.message = "Error al obtener los ingresos";
                }
                if (response.data.Count == 0)
                {
                    response.message = "No se encontraron Ingresos para la fecha dada";
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

        /// <summary>
        /// Filters the list of incomes based on the specified range and date.
        /// </summary>
        /// <param name="incomeList">The list of incomes to filter.</param>
        /// <param name="range">The range of incomes to retrieve (day, week, month, year).</param>
        /// <param name="date">The specific date within the range.</param>
        /// <returns>A filtered list of incomes based on the range and date.</returns>
        private List<IncomeDto> GetIncomesByRange(List<IncomeDto> incomeList, string range, DateTime date)
        {
            switch (range)
            {
                case "day":
                    return incomeList.Where(i => i.date.Date == date.Date).ToList<IncomeDto>();
                case "week":
                    var startOfWeek = date.Date.AddDays(-(int)date.DayOfWeek);
                    var endOfWeek = startOfWeek.AddDays(6);
                    return incomeList.Where(i => i.date.Date >= startOfWeek && i.date.Date <= endOfWeek).ToList<IncomeDto>();
                case "month":
                    return incomeList.Where(i => i.date.Year == date.Year && i.date.Month == date.Month).ToList<IncomeDto>();
                case "year":
                    return incomeList.Where(i => i.date.Year == date.Year).ToList<IncomeDto>();
                default:
                    return incomeList;
            }
        }

        /// <summary>
        /// Modifies an existing income.
        /// </summary>
        /// <param name="income">The updated income information.</param>
        /// <param name="wallet_id">The ID of the wallet associated with the income.</param>
        /// <returns>A <see cref="ResponseIncome"/> object indicating the success of the modification.</returns>
        public async Task<ResponseIncome> ModifyIncome(IncomeDto income, int wallet_id)
        {
            ResponseIncome response = new ResponseIncome();
            try
            {
                response = await _incomeDal.ModifyIncome(income, wallet_id);
                if (!response.code)
                {
                    response.message = "Error al eliminar al ingreso";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }
    }
}
