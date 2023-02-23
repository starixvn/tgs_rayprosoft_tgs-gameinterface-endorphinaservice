using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.EndorphinaService.Core.UnitTest;

[TestClass]
public class WithdrawUnitTest
{
    [TestMethod]
    public async Task 會員提款成功()
    {
        var apiUrl = new Uri("https://test.endorphina.network");
        var username = "4ucmqplxb029";
        var merchantId = "1494";
        var secretKey = "9BC9E97FFCD74B16B87BD6FA82C8C2AF";
        var refTransactionCode = "7a7147218501000000010000af506b2c";
        var amount = 100M;
        var currency = "CNY";
        var url = new Uri(apiUrl, $"/api/casino/withdraw/{username}");

        var response = "{\"id\":80580,\"transactionId\":\"1676958531344\",\"username\":\"4ucmqplxb029\",\"date\":\"2023-02-21T05:48:52Z\",\"amount\":\"100.00\",\"direction\":\"WITHDRAW\",\"currency\":\"CNY\",\"state\":\"APPROVED\",\"comment\":null}";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut.WithdrawAsync(
            apiUrl: apiUrl,
            merchantId: merchantId,
            secretKey: secretKey,
            username: username,
            refTransactionCode: refTransactionCode,
            amount: amount,
            currency: currency,
            Arg.Any<CancellationToken>())
        .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(100M, actual.Balance);
        Assert.AreEqual("7a7147218501000000010000af506b2c", actual.RefTransactionCode);
        Assert.AreEqual("1676958531344", actual.TransactionId);
    }

    [TestMethod]
    public async Task 會員提款餘額不足()
    {
        var apiUrl = new Uri("https://test.endorphina.network");
        var username = "4ucmqplxb029";
        var merchantId = "1494";
        var secretKey = "9BC9E97FFCD74B16B87BD6FA82C8C2AF";
        var refTransactionCode = "7a7147218501000000010000af506b2c";
        var amount = 1000000M;
        var currency = "CNY";
        var url = new Uri(apiUrl, $"/api/casino/withdraw/{username}");

        var response = """
            {
                "code": "INSUFFICIENT_FUNDS_EXCEPTION",
                "message": "Player balance insufficient"
            }
            """;
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.Forbidden, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut.WithdrawAsync(
            apiUrl: apiUrl,
            merchantId: merchantId,
            secretKey: secretKey,
            username: username,
            refTransactionCode: refTransactionCode,
            amount: amount,
            currency: currency,
            Arg.Any<CancellationToken>())
        .ConfigureAwait(false);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public async Task 錯誤幣別()
    {
        var apiUrl = new Uri("https://test.endorphina.network");
        var username = "4ucmqplxb029";
        var merchantId = "1494";
        var secretKey = "9BC9E97FFCD74B16B87BD6FA82C8C2AF";
        var refTransactionCode = "7a7147218501000000010000af506b2c";
        var amount = 1000M;
        var currency = "AAA";
        var url = new Uri(apiUrl, $"/api/casino/withdraw/{username}");

        var response = """
            {
                "code": "INVALID_CURRENCY_EXCEPTION",
                "message": "Invalid currency"
            }
            """;
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.Forbidden, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut.WithdrawAsync(
            apiUrl: apiUrl,
            merchantId: merchantId,
            secretKey: secretKey,
            username: username,
            refTransactionCode: refTransactionCode,
            amount: amount,
            currency: currency,
            Arg.Any<CancellationToken>())
        .ConfigureAwait(false);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public async Task 金額負數()
    {
        var apiUrl = new Uri("https://test.endorphina.network");
        var username = "4ucmqplxb029";
        var merchantId = "1494";
        var secretKey = "9BC9E97FFCD74B16B87BD6FA82C8C2AF";
        var refTransactionCode = "7a7147218501000000010000af506b2c";
        var amount = -1000M;
        var currency = "CNY";
        var url = new Uri(apiUrl, $"/api/casino/withdraw/{username}");

        var response = """
            {
                "code": "ILLEGAL_ARGUMENT_EXCEPTION",
                "message": "Invalid 'amount' argument"
            }
            """;
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.Forbidden, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut.WithdrawAsync(
            apiUrl: apiUrl,
            merchantId: merchantId,
            secretKey: secretKey,
            username: username,
            refTransactionCode: refTransactionCode,
            amount: amount,
            currency: currency,
            Arg.Any<CancellationToken>())
        .ConfigureAwait(false);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public async Task 玩家不存在()
    {
        var apiUrl = new Uri("https://test.endorphina.network");
        var username = "4ucmqplxb000";
        var merchantId = "1494";
        var secretKey = "9BC9E97FFCD74B16B87BD6FA82C8C2AF";
        var refTransactionCode = "7a7147218501000000010000af506b2c";
        var amount = -1000M;
        var currency = "CNY";
        var url = new Uri(apiUrl, $"/api/casino/withdraw/{username}");

        var response = """
            {
                "code": "NOT_FOUND_EXCEPTION",
                "message": "Player not found"
            }
            """;
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.NotFound, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut.WithdrawAsync(
            apiUrl: apiUrl,
            merchantId: merchantId,
            secretKey: secretKey,
            username: username,
            refTransactionCode: refTransactionCode,
            amount: amount,
            currency: currency,
            Arg.Any<CancellationToken>())
        .ConfigureAwait(false);

        Assert.IsNull(actual);
    }
}