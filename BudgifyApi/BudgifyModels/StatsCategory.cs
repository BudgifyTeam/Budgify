using BudgifyModels.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgifyModels
{
    public class StatsCategory
    {
        public string name { get; set; }
        public ExpenseDto[] expenses{ get; set; }
        public double percentile { get; set; }
        public double total { get; set; }

    }
}
 