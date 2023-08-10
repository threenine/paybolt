using System.Diagnostics;
using Models.Http;

namespace PayBolt.DependencyInjection;

/// <summary>
/// Default Http Client Handler
/// </summary>
public class DefaultClientHandler : HttpClientHandler
{
    public DefaultClientHandler(HttpClientHandlerOptions options)
    {
        if (options.EnableSecure)
        {
            var certificate = options.ThumbPrint.ToArray();
            ServerCertificateCustomValidationCallback = (request, cert, chain, errors) =>
            {
                Debug.Assert(chain != null, nameof(chain) + " != null");
                var actual = chain.ChainElements[^1].Certificate;
                return actual.Thumbprint.HexStringToByteArray().SequenceEqual(certificate);
            };
        }
        
        ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true;
    }
}