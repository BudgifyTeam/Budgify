using BudgifyDal;
using BudgifyModels;
using BudgifyModels.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyBll
{
    public class WalletBll
    {
        private readonly UtilsDal _utilsDal;
        private readonly WalletDal _walletDal;
        public WalletBll(AppDbContext db)
        {
            _utilsDal = new UtilsDal(db);
            _walletDal = new WalletDal(db, _utilsDal);
        }

        public Task<ResponseWallet> CreateWallet(int userid, string name, string icon)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseWallet> DeleteWallet(int walletid)
        {
            throw new NotImplementedException();
        }

        public ResponseList<WalletDto> GetWallets(int userid)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseWallet> ModifyWallet(WalletDto wallet, double total, string icon , string name)
        {
            throw new NotImplementedException();
        }
    }
}
