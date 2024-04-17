using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.EndorphinaService.Core.UnitTest;

[TestClass]
public class DepositUnitTest
{
	[TestMethod]
	public async Task 會員存款成功()
	{
		var apiUrl = new Uri("https://test.endorphina.network");
		var username = "4ucmqplxb029";
		var merchantId = "1494";
		var secretKey = "9BC9E97FFCD74B16B87BD6FA82C8C2AF";
		var refTransactionCode = "7a7147218501000000010000af506b2c";
		var amount = 1000M;
		var currency = "CNY";
		var url = new Uri(apiUrl, $"/api/casino/deposit/{username}");

		var response = "{\"id\":80579,\"transactionId\":\"1676957168101\",\"username\":\"4ucmqplxb029\",\"date\":\"2023-02-21T05:26:09Z\",\"amount\":\"1000.00\",\"direction\":\"DEPOSIT\",\"currency\":\"CNY\",\"state\":\"APPROVED\",\"comment\":null}";
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, url.ToString())
						.Respond(HttpStatusCode.OK, new StringContent(response));

		var fakeHttpClient = handler.ToHttpClient();
		var fakeLog = NullLogger<GameApiClient>.Instance;

		var sut = new GameApiClient(fakeHttpClient, fakeLog);

		var actual = await sut.DepositAsync(
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
		Assert.AreEqual(1000M, actual.Balance);
		Assert.AreEqual("7a7147218501000000010000af506b2c", actual.RefTransactionCode);
		Assert.AreEqual("1676957168101", actual.TransactionId);
	}
}
