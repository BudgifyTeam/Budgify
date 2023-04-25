namespace BudgifyModels;

public class Response <T>
{
    public string message { get; set; }
    public List<T> data { get; set; }
}