using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.EndorphinaService.Core.UnitTest;
[TestClass]
public class RequestSessionTokenUnitTest
{
    [TestMethod]
    public async Task 取得Token成功()
    {
        var apiUrl = new Uri("https://test.endorphina.network");        
        var merchantId = "1494";
        var secretKey = "9BC9E97FFCD74B16B87BD6FA82C8C2AF";
        var providerId = "ENDORPHINA";
        var gameId = "endorphina_MinotaurDice";
        var currency = "CNY";
        var username = "4ucmqplxb029";

        var url = new Uri(apiUrl, $"/api/casino/launch/{username}");
        
        var response = """
            {
                "username": "4ucmqplxb029",
                "game": "endorphina_MinotaurDice",
                "provider": "ENDORPHINA",
                "token": "AA8BB60F5A134264BE838CF6EBA7C8F1"
            }
            """;
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut.RequestSessionTokenAsync(
            apiUrl: apiUrl,
            merchantId: merchantId,
            secretKey: secretKey,
            providerId: providerId,
            gameId: gameId,
            currency: currency,
            username: username,
            Arg.Any<CancellationToken>())        
        .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual("AA8BB60F5A134264BE838CF6EBA7C8F1", actual.Token);
        Assert.AreEqual(gameId, actual.Game);
        Assert.AreEqual(username, actual.Username);
    }
}
