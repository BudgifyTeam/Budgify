using BudgifyModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Represents the application's database context.
/// </summary>
/// <remarks>
/// The AppDbContext class is responsible for managing the application's data access and communication with the underlying database.
/// It extends the DbContext class from Entity Framework Core, providing a set of DbSet properties for each entity in the database.
/// The DbSet properties allow querying, inserting, updating, and deleting entities of the corresponding types.
/// 
/// The AppDbContext is configured through the constructor, which takes an instance of DbContextOptions as a parameter.
/// This allows the configuration of the database provider and connection string.
/// 
/// The DbSet properties represent the tables in the database and provide an interface for performing CRUD operations on the entities.
/// The available entities include user, Category, Budget, Wallet, Pocket, Income, and Expense.
/// 
/// Note that the AppDbContext should be registered and used with dependency injection in the application to ensure proper usage and management.
/// </remarks>
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
