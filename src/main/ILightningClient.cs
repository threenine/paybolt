using BoltPay.Lightning;
using BoltPay.Networking;
using BoltPay.Pay;

namespace BoltPay;

public interface ILightningClient 
{
   
    /// <summary>Checks the connectivity.</summary>
    /// <returns>
    ///    True of the connectivity is ok, false otherwise 
    /// </returns>
    Task<Connection> Connect();

    /// <summary>Gets the node / wallet balance.</summary>
    Task<Currency> Balance();
    
    /// <summary>Creates the invoice.</summary>
    Task<Invoice> Create(Currency amount, string description, Options? options = null);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    Task<bool> Paid(string identifier);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="paymentRequest"></param>
    /// <returns>PaymentResponse</returns>
    Task<PaymentResponse> Pay(string paymentRequest);
    
    
    

}