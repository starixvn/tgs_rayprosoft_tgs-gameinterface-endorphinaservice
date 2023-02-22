using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.EndorphinaService.Core.UnitTest;

[TestClass]
public class GameSessionInfoUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://test.endorphina.network");
        var merchantId = "1494";
        var secretKey = "9BC9E97FFCD74B16B87BD6FA82C8C2AF";
        var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";

        var url = new Uri(apiUrl, $"/api/casino/sessions/{token}");

        var response = """
            {
                "id": "AA8BB60F5A134264BE838CF6EBA7C8F1",
                "username": "4ucmqplxb029",
                "game": {
                    "name": "endorphina_MinotaurDice",
                    "provider": "ENDORPHINA",
                    "description": "Minotaur (Dice)",
                    "tag": [
                        "WEBGL"
                    ]
                },
                "currency": "CNY",
                "accountType": "CREDIT",
                "created": "2023-02-21T07:14:46Z",
                "closed": "2023-02-21T07:19:37Z",
                "errorsPending": 0
            }
            """;
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Get, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut.GameSessionInfoAsync(
            apiUrl: apiUrl,
            merchantId: merchantId,
            secretKey: secretKey,
            token: token,
            Arg.Any<CancellationToken>())
        .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual("4ucmqplxb029", actual.Username);
        Assert.AreEqual(token, actual.Id);
    }
}