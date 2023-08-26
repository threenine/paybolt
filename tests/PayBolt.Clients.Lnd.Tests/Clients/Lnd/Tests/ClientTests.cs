using System.ComponentModel;
using System.Net;
using System.Text.Json;
using BoltPay;
using BoltPay.Authentication;
using BoltPay.Clients.Lnd;
using BoltPay.Clients.Lnd.Contracts.v1.Responses;
using FizzWare.NBuilder;
using Moq;
using Moq.Protected;
using Shouldly;

namespace PayBolt.Clients.Lnd.Tests;

public class ClientTests
{
    private const string TestUrlString = "http://localhost:8080";
    private const string SendAsync = "SendAsync";
    private const string TestAlias = "test";

    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandlerHandler = new();
    private readonly Client _client;

    public ClientTests()
    {
        var httpClient = new HttpClient(_mockHttpMessageHandlerHandler.Object);
        httpClient.BaseAddress = new Uri(TestUrlString);
        _client = new Client(httpClient, new NoAuthentication());
    }


    [Fact, Description("Should Connect if Node Alias is not empty")]
    public async Task ShouldConnectIfNodeAliasIsNotEmpty()
    {
        // Arrange

        var getInfo = Builder<GetInfo>.CreateNew()
            .With(x => x.Alias = TestAlias)
            .Build();
        
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(getInfo))
        };
        _mockHttpMessageHandlerHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                SendAsync,
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        //Act
        var result = await _client.NodeInfo();

        // Assert
        _mockHttpMessageHandlerHandler.Protected().Verify(
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
        
        var getInfo = Builder<GetInfo>.CreateNew()
            .With(x => x.Alias = string.Empty)
            .Build();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(getInfo))
        };
        _mockHttpMessageHandlerHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                SendAsync,
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var result = await _client.NodeInfo();

        // Assert
        _mockHttpMessageHandlerHandler.Protected().Verify(
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
        
        var getInfo = Builder<GetInfo>.CreateNew()
            .With(x => x.Alias = string.Empty)
            .Build();
        
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.ServiceUnavailable,
            Content = new StringContent(JsonSerializer.Serialize(getInfo))
        };
        _mockHttpMessageHandlerHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                SendAsync,
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var result = await _client.NodeInfo();

        // Assert
        _mockHttpMessageHandlerHandler.Protected().Verify(
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

         var balance = Builder<Balance>.CreateNew()
            .With(x => x.Total = testSatsBalance)
            .Build();
         
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(balance))
        };
        _mockHttpMessageHandlerHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                SendAsync,
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var result = await _client.Balance();

        // Assert
        _mockHttpMessageHandlerHandler.Protected().Verify(
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
}