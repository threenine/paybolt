using System.Globalization;
using PayBolt;
using PayBolt.Clients.Lnd.Contracts.v1.Responses;
using PayBolt.Networking;
using PayBolt.Pay;
using PayBolt.Authentication;
using PayBolt.Clients.Lnd.Contracts.v1.Requests;
using PayBolt.Clients.Lnd.Contracts.v1.Responses;
using PayBolt.DependencyInjection;
using PayBolt.Exceptions;
using PayBolt.Lightning;

namespace PayBolt.Clients.Lnd;

public class Client : RestServiceBase, ILightningClient
{
    public Client(HttpClient client, AuthenticationBase authentication) : base(client, authentication)
    {
    }

    /// <summary>
    /// Retrieves the node information relating to the connection.
    /// Currently only checks if the alias is empty or not.
    /// </summary>
    /// <returns></returns>
    public async Task<Connection> NodeInfo()
    {
        try
        {
            var result = await Get<GetInfo>(Routes.GetInfo);
            if (string.IsNullOrEmpty(result.Alias)) return new Connection(Result.Error, Resources.LndConnectFailure);
        }
        catch (Exception e)
        {
            return new Connection(Result.Error, e.Message);
        }

        return new Connection(Result.Ok);
    }

    /// <summary>
    /// Get the wallet balance
    /// </summary>
    /// <returns></returns>
    public async Task<Currency> Balance()
    {
        var result = await Get<Balance>(Routes.Balance);
        var sats = result?.Total ?? 0;
        return Currency.FromSats(sats);
    }

  /// <summary>
  /// Create an invoice to receive payment
  /// </summary>
  /// <param name="amount"></param>
  /// <param name="description"></param>
  /// <param name="options"></param>
  /// <returns></returns>
  /// <exception cref="PayBoltException"></exception>
    public async Task<Invoice> Create(Currency amount, string description, Options? options = null)
    {
        var strAmount = ((long)amount.ToSats()).ToString(CultureInfo.InvariantCulture);
        var strExpiry = options?.ToExpiryString();
        
        var request = new LnrpcInvoice
        {
            Value = strAmount,
            Memo = description,
            Expiry = strExpiry ?? string.Empty,
            Private = false
        };
        
        var response = await Post<AddInvoice>(Routes.Invoice,
            request);
        
        if(string.IsNullOrEmpty(response.PaymentRequest) || response.RHash == null)
            throw new PayBoltException(Resources.LndCreateInvoiceFailure);
        
        return response.ToLightningInvoice(amount, description, options);
    }

    public Task<bool> Paid(string identifier)
    {
        throw new NotImplementedException();
    }

    public Task<Payment> Pay(string paymentRequest)
    {
        throw new NotImplementedException();
    }
}