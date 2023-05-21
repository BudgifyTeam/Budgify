using BudgifyModels;
using BudgifyModels.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyDal
{
    public class HistoryDal
    {
        private readonly AppDbContext _appDbContext;
        private readonly UtilsDal _utilsDal;

        public HistoryDal(AppDbContext db, UtilsDal fn)
        {
            _appDbContext = db;
            _utilsDal = fn;
        }

        public async Task<HistoryDto> GetHistory(int userid, DateTime date, string range)
        {
            //ToDo
            return null;
        }
    }
}
