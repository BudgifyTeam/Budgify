using BudgifyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyDal
{
    public class BudgetDal
    {
        private readonly AppDbContext _appDbContext;
        private readonly UtilsDal _utilsDal;

        public BudgetDal(AppDbContext db, UtilsDal fn)
        {
            _appDbContext = db;
            _utilsDal = fn;
        }

        public async Task<Response<Budget>> updateBudget(int userId)
        {
            Response<Budget> response = new Response<Budget>();
            try {
                var budget = _appDbContext.budget.FirstOrDefault(b => b.users_id == userId);
                var incomes = _utilsDal.GetIncomesByUserId(userId);
                budget.value = incomes.Sum(i => i.value);
                await _appDbContext.SaveChangesAsync();
                response.data = budget;
                response.code = true;
                response.message = "se actualizó el presupesto correctamente";
            }
            catch (Exception ex)
            {
                response.code = false;
                response.message = ex.Message;
            }
            return response;
        }
    }
}
