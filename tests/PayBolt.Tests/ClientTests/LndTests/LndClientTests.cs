using System.ComponentModel;
using System.Net;
using System.Text.Json;
using BoltPay.Exceptions;
using BoltPay.Lnd;
using BoltPay.Lnd.Responses;
using Shouldly;

namespace BoltPay.Tests.ClientTests.LndTests;

public class LndClientTests
{
    
    private const string TestUrlString = "http://localhost:8080";
    
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
         var lndClient = LndClient.Create(TestUrlString, httpClient: client);

        var result = await lndClient.Connect();
        
        result.ShouldSatisfyAllConditions(
            _ => mockHttpMessageHandlerHandler.Requests.Count.ShouldBe(1),
            _ => mockHttpMessageHandlerHandler.Requests[0].RequestUri.ShouldBeEquivalentTo(new Uri($"{TestUrlString}/v1/getinfo")),
            _ => result.Result.ShouldBe(Result.Ok)
            );
        
       
   }
   
   [Fact, Description("Should not Connect if Node Alias is empty")] 
   public async Task ShouldNotConnectIfNodeIsEmpty()
   {
       var mockHttpMessageHandlerHandler = new MockHttpMessageHandler(
           (JsonSerializer.Serialize(new GetInfo { }), HttpStatusCode.OK));
       
       var client = new HttpClient(mockHttpMessageHandlerHandler);
       var lndClient = LndClient.Create(TestUrlString, httpClient: client);

       var result = await lndClient.Connect();
        
       result.ShouldSatisfyAllConditions(
           _ => mockHttpMessageHandlerHandler.Requests.Count.ShouldBe(1),
           _ => mockHttpMessageHandlerHandler.Requests[0].RequestUri.ShouldBeEquivalentTo(new Uri($"{TestUrlString}/v1/getinfo")),
           _ => result.Result.ShouldBe(Result.Error),
           _ => result.Error.ShouldNotBeNull()
       );
   }

   [Fact, Description("Should return error if the connection request fails")]
   public async Task ShouldReturnErrorIfConnectionFails()
   {
       var mockHttpMessageHandlerHandler = new MockHttpMessageHandler(
           (JsonSerializer.Serialize(new GetInfo { }), HttpStatusCode.ServiceUnavailable));
       
       var client = new HttpClient(mockHttpMessageHandlerHandler);
       var lndClient = LndClient.Create(TestUrlString, httpClient: client);

       var result = await lndClient.Connect();
        
       result.ShouldSatisfyAllConditions(
           _ => mockHttpMessageHandlerHandler.Requests.Count.ShouldBe(1),
           _ => mockHttpMessageHandlerHandler.Requests[0].RequestUri.ShouldBeEquivalentTo(new Uri($"{TestUrlString}/v1/getinfo")),
           _ => result.Result.ShouldBe(Result.Error),
           _ => result.Error.ShouldNotBeNull(),
           _ => result.Error.ShouldBe("Response status code does not indicate success: 503 (Service Unavailable).")
       );
       
   }
    
}