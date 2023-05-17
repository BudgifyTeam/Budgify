using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyDal
{
    public class ExpenseDal
    {
        private readonly AppDbContext _appDbContext;
        private readonly UtilsDal _utilsDal;
        private readonly BudgetDal _budgetDal;

        public ExpenseDal(AppDbContext db, UtilsDal fn, BudgetDal bd)
        {
            _appDbContext = db;
            _utilsDal = fn;
            _budgetDal = bd;
        }
    }
}
