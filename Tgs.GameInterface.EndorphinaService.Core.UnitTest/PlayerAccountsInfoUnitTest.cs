using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.EndorphinaService.Core.UnitTest;

[TestClass]
public class PlayerAccountsInfoUnitTest
{
	[TestMethod]
	public async Task 玩家存在且有Deposit()
	{
		var apiUrl = new Uri("https://test.endorphina.network");
		var username = "4ucmqplxb029";
		var merchantId = "1494";
		var secretKey = "9BC9E97FFCD74B16B87BD6FA82C8C2AF";
		var currency = "CNY";
		var url = new Uri(apiUrl, $"/api/casino/player/{username}/accounts");

		var response = """
            [
                {
                    "balance": "1000.00",
                    "currency": "CNY",
                    "type": "CASH"
                }
            ]
            """;
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
			currency: currency,
			Arg.Any<CancellationToken>())
		.ConfigureAwait(false);

		Assert.IsNotNull(actual);
		Assert.IsTrue(actual.IsPlayerExist);
		Assert.AreEqual(1000M, actual.Balance);
	}

	[TestMethod]
	public async Task 玩家不存在()
	{
		var apiUrl = new Uri("https://test.endorphina.network");
		var username = "4ucmqplxhfoe";
		var merchantId = "1494";
		var secretKey = "9BC9E97FFCD74B16B87BD6FA82C8C2AF";
		var currency = "CNY";

		var url = new Uri(apiUrl, $"/api/casino/player/{username}/accounts");

		var response = """
            {
                "code": "NOT_FOUND_EXCEPTION",
                "message": "Player not found"
            }
            """;
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Get, url.ToString())
						.Respond(HttpStatusCode.NotFound, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut.PlayerAccountsInfoAsync(
			apiUrl: apiUrl,
			merchantId: merchantId,
			secretKey: secretKey,
			username: username,
			currency: currency,
			Arg.Any<CancellationToken>())
		.ConfigureAwait(false);

		Assert.IsFalse(actual.IsPlayerExist);
		Assert.IsNull(actual.Balance);
	}

	[TestMethod]
	public async Task 玩家存在但沒Deposit()
	{
		var apiUrl = new Uri("https://test.endorphina.network");
		var username = "4ucmqplxkuyd";
		var merchantId = "1494";
		var secretKey = "9BC9E97FFCD74B16B87BD6FA82C8C2AF";
		var currency = "CNY";

		var url = new Uri(apiUrl, $"/api/casino/player/{username}/accounts");

		var response = "[]";
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
			currency: currency,
			Arg.Any<CancellationToken>())
		.ConfigureAwait(false);

		Assert.IsTrue(actual.IsPlayerExist);
		Assert.AreEqual(0M, actual.Balance);
	}
}
