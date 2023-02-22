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
}