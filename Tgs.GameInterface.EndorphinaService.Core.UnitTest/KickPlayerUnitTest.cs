using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.EndorphinaService.Core.UnitTest;

[TestClass]
public class KickPlayerUnitTest
{
	[TestMethod]
	public async Task 踢玩家成功()
	{
		var apiUrl = new Uri("https://test.endorphina.network");
		var merchantId = "1494";
		var secretKey = "9BC9E97FFCD74B16B87BD6FA82C8C2AF";
		var username = "4ucmqplxb029";

		var url = new Uri(apiUrl, $"/api/casino/kick/{username}");

		var response = """
            {
                "username": "4ucmqplxb029",
                "created": "2023-02-21T03:07:56Z",
                "active": true
            }
            """;
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, url.ToString())
						.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut.KickPlayerAsync(
			apiUrl: apiUrl,
			merchantId: merchantId,
			secretKey: secretKey,
			username: username,
			Arg.Any<CancellationToken>())
		.ConfigureAwait(false);

		Assert.IsTrue(actual);
	}
}
