using BudgifyApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgifyApi.Data
{
    public class UsersDB : DbContext
    {
        public UsersDB(DbContextOptions<UsersDB> options) : base(options) 
        {
            
        }
        public DbSet<Usuario> usuarios => Set<Usuario>();

    }
}
