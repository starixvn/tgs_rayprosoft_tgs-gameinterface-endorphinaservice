using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.EndorphinaService.Core.UnitTest;

[TestClass]
public class PlayerRegistrationUnitTest
{
    [TestMethod]
    public async Task 玩家註冊成功()
    {
        var apiUrl = new Uri("https://test.endorphina.network");
        var username = "4ucmqplxkuyd";
        var merchantId = "1494";
        var secretKey = "9BC9E97FFCD74B16B87BD6FA82C8C2AF";

        var url = new Uri(apiUrl, $"/api/casino/player/{username}");

        var response = "{\"username\":\"4ucmqplxkuyd\",\"created\":\"2023-02-21T05:32:37Z\",\"active\":true}";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        await sut.PlayerRegistrationAsync(
            apiUrl: apiUrl,
            merchantId: merchantId,
            secretKey: secretKey,
            username: username,
            Arg.Any<CancellationToken>())
        .ConfigureAwait(false);
    }
}