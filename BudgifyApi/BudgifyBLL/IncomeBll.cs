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
    public class IncomeBll
    {
        private readonly UtilsDal _utilsDal;
        private readonly IncomeDal _incomeDal;
        private readonly BudgetDal _budgifyDal;
        public IncomeBll(AppDbContext db)
        {
            _utilsDal = new UtilsDal(db);
            _budgifyDal = new BudgetDal(db, _utilsDal);
            _incomeDal = new IncomeDal(db, _utilsDal, _budgifyDal);
        }

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

        public ResponseList<IncomeDto> GetIncomes(int userid, string range)//range{day, week, month, year}
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
