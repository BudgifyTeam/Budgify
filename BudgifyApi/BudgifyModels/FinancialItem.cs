using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgifyModels
{
    public class FinancialItem
    {
        public string name { get; set; }
        public double value { get; set; }
        public DateTime date { get; set; }
        public string type { get; set; }
        public string? category { get; set; }

    }
}
 