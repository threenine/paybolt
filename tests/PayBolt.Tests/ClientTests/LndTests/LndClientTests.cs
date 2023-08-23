using System.ComponentModel;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using BoltPay.Exceptions;
using BoltPay.Lnd;
using BoltPay.Lnd.Responses;
using PayBolt.DependencyInjection;
using Shouldly;

namespace BoltPay.Tests.ClientTests.LndTests;

public class LndClientTests
{
    [Fact, Description("Should throw an exception if the URI is invalid")]
    public void ShouldThrowAnExceptionIfTheUriIsInvalid()
    {
       Should.Throw<PayBoltException>(() => LndClient.Create("bad_uri"));
    }
    
    [Fact, Description("Should throw an exception if the URI is null")]
    public void ShouldThrowAnExceptionIfTheUriIsNull()
    {
        Should.Throw<PayBoltException>(() => LndClient.Create(null!));
    }
    
   [Fact, Description("Should Connect if Node Alias is not empty")] 
   public async Task ShouldConnectIfNodeAliasIsNotEmpty()
   {
       var mockHttpMessageHandlerHandler = new MockHttpMessageHandler(
           (JsonSerializer.Serialize(new GetInfo { Alias = "test" }), HttpStatusCode.OK));
       
       var client = new HttpClient(mockHttpMessageHandlerHandler);
         var lndClient = LndClient.Create("http://localhost:8080", httpClient: client);

        var result = await lndClient.Connect();
        
        result.ShouldSatisfyAllConditions(
            _ => mockHttpMessageHandlerHandler.Requests.Count.ShouldBe(1),
            _ => mockHttpMessageHandlerHandler.Requests[0].RequestUri.ShouldBeEquivalentTo(new Uri("http://localhost:8080/v1/getinfo")),
            _ => result.Result.ShouldBe(Result.Ok)
            );
   }
    
}