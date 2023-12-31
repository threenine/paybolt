/*using System.ComponentModel;
using System.Net;
using System.Text.Json;
using PayBolt.Exceptions;
using PayBolt.Lnd;
using PayBolt.Lnd.Responses;
using Moq;
using Moq.Protected;
using Shouldly;

namespace PayBolt.Tests.ClientTests.LndTests;

public class LndClientTests
{
    private const string TestUrlString = "http://localhost:8080";
    private const string SendAsync = "SendAsync";
    private const string TestAlias = "test";

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
        
        // Arrange
        var mockHttpMessageHandlerHandler = new Mock<HttpMessageHandler>();

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(new GetInfo { Alias = TestAlias }))
        };
        mockHttpMessageHandlerHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                SendAsync,
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        //Act
        var client = new HttpClient(mockHttpMessageHandlerHandler.Object);
        var lndClient = LndClient.Create(TestUrlString, httpClient: client);

        var result = await lndClient.Connect();
        
        // Assert
        mockHttpMessageHandlerHandler.Protected().Verify(
            SendAsync,
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get
                && req.RequestUri == new Uri(string.Concat(TestUrlString, Routes.GetInfo))
            ),
            ItExpr.IsAny<CancellationToken>()
        );

        result.ShouldSatisfyAllConditions(
            _ => result.Result.ShouldBe(Result.Ok)
        );
        
       
    }

    [Fact, Description("Should not Connect if Node Alias is empty")]
    public async Task ShouldNotConnectIfNodeIsEmpty()
    {
        // Arrange
        var mockHttpMessageHandlerHandler = new Mock<HttpMessageHandler>();

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(new GetInfo { Alias = string.Empty }))
        };
        mockHttpMessageHandlerHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                SendAsync,
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var client = new HttpClient(mockHttpMessageHandlerHandler.Object);
        var lndClient = LndClient.Create(TestUrlString, httpClient: client);

        var result = await lndClient.Connect();
        
        // Assert
        mockHttpMessageHandlerHandler.Protected().Verify(
            SendAsync,
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get
                && req.RequestUri == new Uri(string.Concat(TestUrlString, Routes.GetInfo))
            ),
            ItExpr.IsAny<CancellationToken>()
        );

        result.ShouldSatisfyAllConditions(
            _ => result.Result.ShouldBe(Result.Error),
            _ => result.Error.ShouldNotBeNull()
        );
        
      
    }

    [Fact, Description("Should return error if the connection request fails")]
    public async Task ShouldReturnErrorIfConnectionFails()
    {
        // Arrange
        var mockHttpMessageHandlerHandler = new Mock<HttpMessageHandler>();

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.ServiceUnavailable,
            Content = new StringContent(JsonSerializer.Serialize(new GetInfo { Alias = string.Empty }))
        };
        mockHttpMessageHandlerHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                SendAsync,
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var client = new HttpClient(mockHttpMessageHandlerHandler.Object);
        var lndClient = LndClient.Create(TestUrlString, httpClient: client);

        var result = await lndClient.Connect();
        
        // Assert
        mockHttpMessageHandlerHandler.Protected().Verify(
            SendAsync,
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get
                && req.RequestUri == new Uri(string.Concat(TestUrlString, Routes.GetInfo))
            ),
            ItExpr.IsAny<CancellationToken>()
        );

        result.ShouldSatisfyAllConditions(
            _ => result.Result.ShouldBe(Result.Error),
            _ => result.Error.ShouldNotBeNull(),
            _ => result.Error.ShouldBe("Response status code does not indicate success: 503 (Service Unavailable).")
        );
    }

    [Fact, Description("Should return a balance value from a Wallet Balance request")]
    public async Task ShouldReturnWalletBalance()
    {
        // Arrange
        const long testSatsBalance = 10000;
      

        var mockHttpMessageHandlerHandler = new Mock<HttpMessageHandler>();

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(new Balance { Total = testSatsBalance }))
        };
        mockHttpMessageHandlerHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                SendAsync,
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var client = new HttpClient(mockHttpMessageHandlerHandler.Object);
        var lndClient = LndClient.Create(TestUrlString, httpClient: client);

        var result = await lndClient.Balance();
        
        // Assert
        mockHttpMessageHandlerHandler.Protected().Verify(
            SendAsync,
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Get
                && req.RequestUri == new Uri(string.Concat(TestUrlString, Routes.Balance))
            ),
            ItExpr.IsAny<CancellationToken>()
        );
        
        result.ShouldSatisfyAllConditions(
            _ => result.ToSats().ShouldBe(testSatsBalance)
        );
        
    }
}*/