using System.Runtime.InteropServices.JavaScript;

namespace PayBolt.Exceptions;

public class PayBoltException : Exception
{
    public Error Error { get; }
    public string Detail { get; }

    public PayBoltException()
    {
    }
    
    public PayBoltException( string message) : base(message)
    {
    }

    public PayBoltException(Error error, string? detail = null, Exception? innerException = null) : base(detail, innerException)
    {
        Error = error;
        Detail = detail ?? string.Empty;
    }

    public PayBoltException(string message, Exception innerException) : base(message, innerException)
    {
    }
    
}