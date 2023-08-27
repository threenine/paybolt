namespace PayBolt.Pay;

public class Payment
{
    public Result Result { get; set; }
    public string Error { get; set; }

    public Payment(Result result)
    {
        Result = result;
    }

    public Payment(Result result, string error)
    {
        Result = result;
        Error = error;
    }
    
}