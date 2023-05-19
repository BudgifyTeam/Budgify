using BudgifyModels;
using BudgifyModels.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyDal
{
    public class WalletDal
    {
        private readonly AppDbContext _appDbContext;
        private readonly UtilsDal _utilsDal;

        public WalletDal(AppDbContext db, UtilsDal utils)
        {
            _appDbContext = db;
            _utilsDal = utils;
        }

        public Wallet[] AsignUserToWallet(Wallet[] list)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseWallet> CreateWallet(Wallet wallet)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseWallet> DeleteWallet(int walletid)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseWallet> ModifyWallet(WalletDto wallet, double total, string icon, string name)
        {
            throw new NotImplementedException();
        }
    }
}
