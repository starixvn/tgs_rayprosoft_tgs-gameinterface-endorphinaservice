using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.EndorphinaService.Core.Exceptions;

namespace Tgs.GameInterface.EndorphinaService.Core.UnitTest;

[TestClass]
public class GameSessionStatisticsUnitTest
{
	[TestMethod]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://test.endorphina.network");
		var merchantId = "1494";
		var secretKey = "9BC9E97FFCD74B16B87BD6FA82C8C2AF";
		var token = "AA8BB60F5A134264BE838CF6EBA7C8F1";

		var url = new Uri(apiUrl, $"/api/casino/stats/{token}");

		var response = """
            {
                "bet": "2.00",
                "win": "0.50",
                "games": 4,
                "jackpotWin": "0.00",
                "jackpots": 0,
                "deposit": "2.00",
                "withdraw": "0.50",
                "errorsPending": 0
            }
            """;
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Get, url.ToString())
						.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut.GameSessionStatisticsAsync(
			apiUrl: apiUrl,
			merchantId: merchantId,
			secretKey: secretKey,
			token: token,
			Arg.Any<CancellationToken>())
			.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.AreEqual(2M, actual.Bet);
	}

	[TestMethod]
	public void Session_Not_Found()
	{
		var apiUrl = new Uri("https://test.endorphina.network");
		var merchantId = "1494";
		var secretKey = "9BC9E97FFCD74B16B87BD6FA82C8C2AF";
		var token = "AA8BB60F5A134264BE838CF6EBA7C8F0";

		var url = new Uri(apiUrl, $"/api/casino/stats/{token}");

		var response = """
            {
                "code": "NOT_FOUND_EXCEPTION",
                "message": "Session not found"
            }
            """;
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Get, url.ToString())
						.Respond(HttpStatusCode.NotFound, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		_ = Assert.ThrowsExceptionAsync<GameProviderResponseErrorException>(
			async () => await sut.GameSessionStatisticsAsync(
			apiUrl: apiUrl,
			merchantId: merchantId,
			secretKey: secretKey,
			token: token,
			Arg.Any<CancellationToken>())
			.ConfigureAwait(false));
	}
}
