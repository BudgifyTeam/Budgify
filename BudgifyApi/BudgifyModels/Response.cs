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