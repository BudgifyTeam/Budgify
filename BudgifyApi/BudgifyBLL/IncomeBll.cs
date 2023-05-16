using BudgifyDal;
using BudgifyModels;
using BudgifyModels.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace BudgifyBll
{
    public class IncomeBll
    {
        private readonly UtilsDal _utilsDal;
        private readonly IncomeDal _incomeDal;
        public IncomeBll(AppDbContext db)
        {
            _utilsDal = new UtilsDal(db);
            _incomeDal = new IncomeDal(db, _utilsDal);
        }

        public async Task<Response<IncomeDto>> CreateIncome(int userid, double value, DateTime date)
        {
           Response<IncomeDto> response = new Response<IncomeDto>();
            try {
                var newIncome = new Income()
                {
                    date = date,
                    income_id = 0,
                    status = "active",
                    users_id = userid,
                    value = value
                };
                response = await _incomeDal.CreateIncome(newIncome);
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

        public async Task<Response<IncomeDto>> DeleteIncome(int userid, int incomeid)
        {
            Response<IncomeDto> response = new Response<IncomeDto>();
            try
            {
                response = await _incomeDal.DeleteIncome(userid, incomeid);
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

                var incomeList = list.Select(GetIncomeDto).ToList();
                response.data = GetIncomesByRange(incomeList, range, DateTime.Today).ToList();
                if (!response.code)
                {
                    response.message = "Error al obtener los ingresos";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
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

                var incomeList = list.Select(GetIncomeDto).ToList();
                response.data = GetIncomesByRange(incomeList, range, date).ToList();
                if (!response.code)
                {
                    response.message = "Error al obtener los ingresos";
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
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

        public async Task<Response<IncomeDto>> ModifyIncome(IncomeDto income)
        {
            Response<IncomeDto> response = new Response<IncomeDto>();
            try
            {
                response = await _incomeDal.ModifyIncome(income);
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
        public IncomeDto GetIncomeDto(Income income)
        {
            return new IncomeDto
            {
                value = income.value,
                date = income.date,
                income_id = income.income_id,
                wallet = income.wallet.name
            };
        }
    }
}
