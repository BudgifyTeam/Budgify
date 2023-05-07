using System.ComponentModel.DataAnnotations;

namespace BudgifyModels
{
    public class Session //Only for inside use - not To DataBase
    {
        public int Id { get; set; }
        public string? User_id { get; set; }
        public string? Budget_id { get; set; }
        public Category[]? Categories { get; set; }
        public Income[]? Incomes { get; set; }
        public Expense[]? Expenses { get; set; }
    }
}
