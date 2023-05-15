using BudgifyModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgifyDal
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {
            
        }
        public DbSet<user> users { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Budget> budget { get; set; }
        public DbSet<Wallet> wallets { get; set; }
        public DbSet<Pocket> pockets { get; set; }
        public DbSet<Income> incomes { get; set; }
        public DbSet<Expense> expenses { get; set; }
    }
}
