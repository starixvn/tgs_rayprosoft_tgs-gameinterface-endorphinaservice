using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.EndorphinaService.Core.UnitTest;

[TestClass]
public class SinglePaymentUnitTest
{
    [TestMethod]
    public async Task 查詢轉帳紀錄成功()
    {
        var apiUrl = new Uri("https://test.endorphina.network");
        var merchantId = "1494";
        var secretKey = "9BC9E97FFCD74B16B87BD6FA82C8C2AF";
        var transactionId = "1676958531344";

        var url = new Uri(apiUrl, $"/api/casino/payments/{transactionId}");

        var response = "{\"id\":80580,\"transactionId\":\"1676958531344\",\"username\":\"4ucmqplxb029\",\"date\":\"2023-02-21T05:48:52Z\",\"amount\":\"100.00\",\"direction\":\"WITHDRAW\",\"currency\":\"CNY\",\"state\":\"APPROVED\",\"comment\":null}";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Get, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut.SinglePaymentAsync(
            apiUrl: apiUrl,
            merchantId: merchantId,
            secretKey: secretKey,
            transactionId: transactionId,
            Arg.Any<CancellationToken>())
        .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual);
    }
}