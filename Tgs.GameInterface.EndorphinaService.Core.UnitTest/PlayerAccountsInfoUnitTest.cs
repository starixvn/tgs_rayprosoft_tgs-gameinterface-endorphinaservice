using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.EndorphinaService.Core.UnitTest;

[TestClass]
public class PlayerAccountsInfoUnitTest
{
    [TestMethod]
    public async Task 取得玩家帳號資訊成功()
    {
        var apiUrl = new Uri("https://test.endorphina.network");
        var username = "4ucmqplxb029";
        var merchantId = "1494";
        var secretKey = "9BC9E97FFCD74B16B87BD6FA82C8C2AF";

        var url = new Uri(apiUrl, $"/api/casino/player/{username}/accounts");

        var response = "[{\"balance\":\"1000.00\",\"currency\":\"CNY\",\"type\":\"CASH\"}]";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Get, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut.PlayerAccountsInfoAsync(
            apiUrl: apiUrl,
            merchantId: merchantId,
            secretKey: secretKey,
            username: username,
            Arg.Any<CancellationToken>())
        .ToArrayAsync(Arg.Any<CancellationToken>())
        .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(1, actual.Length);
    }
}