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
        public DbSet<User> user { get; set; }
        public DbSet<Budget> budgets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Pocket> Pockets { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
    }
}
