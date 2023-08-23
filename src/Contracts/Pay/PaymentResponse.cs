namespace BoltPay.Pay;

public class PaymentResponse
{
    public Result Result { get; set; }
    public string Error { get; set; }

    public PaymentResponse(Result result)
    {
        Result = result;
    }

    public PaymentResponse(Result result, string error)
    {
        Result = result;
        Error = error;
    }
    
}