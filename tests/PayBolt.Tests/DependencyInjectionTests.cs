using System.ComponentModel;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Models.Http;
using PayBolt.DependencyInjection;
using Shouldly;

namespace PayBolt.Tests;

public class DependencyInjectionTests
{
    [Fact, Description("Allow Insecure connections")]
    public void Should_Add_Certificate_Enable_Secure()
    {
        // Arrange
        // Setting the EnableSecure option to false enables insecure connections
        var handler = new DefaultClientHandler(new HttpClientHandlerOptions
        {
            EnableSecure = false
        });

        var expected =
            handler.ServerCertificateCustomValidationCallback(null, null, null,
                SslPolicyErrors.None);
        
        expected.ShouldBeTrue();
    }
    
    [Fact, Description("Enable secure connections with a certificate thumbprint send bytes")]
    public void Should_Add_Certificate_Thumbprint_with_bytes()
    {
        var handler = new DefaultClientHandler(new HttpClientHandlerOptions
        {
            EnableSecure = true, 
            ThumbPrint = new byte[] { 0x01, 0x02, 0x03 }
        });

        var expected =
            handler.ServerCertificateCustomValidationCallback(null, null, null,
                SslPolicyErrors.None);
        
        expected.ShouldBeTrue();
    }
    
    [Fact, Description("Enable secure connections with a self signed certificate thumbprint")]
    public void Should_Add_Certificate_Thumbprint()
    {
        var handler = new DefaultClientHandler(new HttpClientHandlerOptions
        {
            EnableSecure = true, 
            ThumbPrint = BuildSelfSignedCertificate().Thumbprint.HexStringToByteArray()
        });

        var expected =
            handler.ServerCertificateCustomValidationCallback(null, null, null,
                SslPolicyErrors.None);
        
        expected.ShouldBeTrue();
    }
    
    
    
    private X509Certificate2 BuildSelfSignedCertificate()
    {
        
        const string referenceRecord = "1.2.840.10045.3.1.7"; // https://oidref.com/1.2.840.10045.3.1

         const string subject = "Self-Signed-Cert-Example";

        var ecdsa = ECDsa.Create(ECCurve.CreateFromValue(referenceRecord));

        var certRequest = new CertificateRequest($"CN={subject}", ecdsa, HashAlgorithmName.SHA256);

        //add extensions to the request (just as an example)
        //add keyUsage
        certRequest.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, true));

        var generatedCert = certRequest.CreateSelfSigned(DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now.AddYears(10)); // generate the cert and sign!
          // Create with PFX to avoid "security credentials not found" error on Windows
        var pfxGeneratedCert = new X509Certificate2(generatedCert.Export(X509ContentType.Pfx)); 

        return pfxGeneratedCert;
    }
}