using BudgifyModels.Dto;

namespace BudgifyModels;

public class ResponseList <T>
{
    public string? message { get; set; }
    public List<T>? data { get; set; }
    public Boolean code { get; set; }
}

public class Response<T> {
    public string? message { get; set; }
    public T? data { get; set; }
    public Boolean code { get; set; }
}

public class ResponseError
{
    public string? message { get; set; }
    public int? code { get; set; }
}

public class ResponseIncome {
    public string? message { get; set; }
    public Boolean code { get; set; }
    public IncomeDto? income { get; set; }
    public double? newBudget { get; set; }
}
