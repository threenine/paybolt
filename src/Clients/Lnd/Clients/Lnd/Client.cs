using BoltPay.Authentication;
using BoltPay.Clients.Lnd.Contracts.v1.Responses;
using BoltPay.Lightning;
using BoltPay.Networking;
using BoltPay.Pay;
using PayBolt.Clients;

namespace BoltPay.Clients.Lnd;

public class Client : RestServiceBase, ILightningClient
{
    public Client(HttpClient client, AuthenticationBase authentication) : base(client, authentication)
    {
    }

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

    public async Task<Currency> Balance()
    {
        var result = await Get<Balance>(Routes.Balance);
        var sats = result?.Total ?? 0;
        return Currency.FromSats(sats);
    }

    public Task<Invoice> Create(Currency amount, string description, Options? options = null)
    {
        throw new NotImplementedException();
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