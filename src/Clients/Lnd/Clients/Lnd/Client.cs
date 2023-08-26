using System.Globalization;
using BoltPay.Authentication;
using BoltPay.Clients.Lnd.Contracts.v1.Requests;
using BoltPay.Clients.Lnd.Contracts.v1.Responses;
using BoltPay.Exceptions;
using BoltPay.Lightning;
using BoltPay.Networking;
using BoltPay.Pay;
using PayBolt.Clients;
using PayBolt.DependencyInjection;

namespace BoltPay.Clients.Lnd;

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

    public async Task<Invoice> Create(Currency amount, string description, Options? options = null)
    {
        var strAmount = ((long)amount.ToSats()).ToString(CultureInfo.InvariantCulture);
        var strExpiry = options.ToExpiryString();


        var request = new LnrpcInvoice
        {
            Value = strAmount,
            Memo = description,
            Expiry = strExpiry,
            Private = false
        };
        
        var response = await Post<AddInvoice>(Routes.Invoice,
            request);
        
        if(string.IsNullOrEmpty(response.PaymentRequest) || response.Hash == null)
            throw new PayBoltException(Resources.LndCreateInvoiceFailure);
        
        return response.ToLightningInvoice(amount, description, options);
    }

    public Task<bool> Paid(string identifier)
    {
        throw new NotImplementedException();
    }

    public Task<PaymentResponse> Pay(string paymentRequest)
    {
        throw new NotImplementedException();
    }
}