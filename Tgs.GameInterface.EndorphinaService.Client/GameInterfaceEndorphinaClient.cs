using Tgs.GameInterface.EndorphinaService.Protos;

namespace Tgs.GameInterface.EndorphinaService.Client;

internal class GameInterfaceEndorphinaClient(GameInterfaceEndorphinaService.GameInterfaceEndorphinaServiceClient client) : IGameInterfaceEndorphinaClient
{
	public async ValueTask<string> CheckingServiceAsync(
		string machineName,
		CancellationToken cancellationToken = default)
	{
		var request = new CheckingServiceRequest
		{
			MachineName = machineName,
		};

		var reply = await client
			.CheckingServiceAsync(
				request: request,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.MachineName;
	}

	public async ValueTask<FundReply> FundInAsync(
		Uri apiUrl,
		string merchantId,
		string secretKey,
		string username,
		string currency,
		string refTransactionCode,
		decimal amount,
		CancellationToken cancellationToken = default)
	{
		var request = new FundRequest
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			SecretKey = secretKey,
			Username = username,
			Currency = currency,
			RefTransactionCode = refTransactionCode,
			Amount = amount,
		};

		return await client
			.FundInAsync(request, cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<FundReply> FundOutAsync(
		Uri apiUrl,
		string merchantId,
		string secretKey,
		string username,
		string currency,
		string refTransactionCode,
		decimal amount, CancellationToken cancellationToken = default)
	{
		var request = new FundRequest
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			SecretKey = secretKey,
			Username = username,
			Currency = currency,
			RefTransactionCode = refTransactionCode,
			Amount = amount,
		};

		return await client
			.FundOutAsync(request, cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<FundStatusReply> FundStatusAsync(
		Uri apiUrl,
		string merchantId,
		string secretKey,
		string transactionId,
		CancellationToken cancellationToken = default)
	{
		var request = new FundStatusRequest
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			SecretKey = secretKey,
			TransactionId = transactionId,
		};

		return await client
			.FundStatusAsync(request, cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<GameUrlReply> GameUrlAsync(
		Uri apiUrl,
		string merchantId,
		string secretKey,
		string gameId,
		string currency,
		string username,
		string language,
		CancellationToken cancellationToken = default)
	{
		var request = new GameUrlRequest
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			SecretKey = secretKey,
			GameId = gameId,
			Currency = currency,
			Username = username,
			Language = language,
		};

		return await client
			.GameUrlAsync(request, cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
		Uri apiUrl,
		string merchantId,
		string secretKey,
		string username,
		string currency,
		CancellationToken cancellationToken = default)
	{
		var request = new PlayerBalanceRequest
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			SecretKey = secretKey,
			Username = username,
			Currency = currency,
		};

		return await client
			.PlayerBalanceAsync(request, cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask PlayerCreateAsync(
		Uri apiUrl,
		string merchantId,
		string secretKey,
		string username,
		CancellationToken cancellationToken = default)
	{
		var request = new PlayerCreateRequest
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			SecretKey = secretKey,
			Username = username,
		};

		_ = await client
			.PlayerCreateAsync(request, cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask PlayerKickAsync(
		Uri apiUrl,
		string merchantId,
		string secretKey,
		string username,
		CancellationToken cancellationToken = default)
	{
		var request = new PlayerKickRequest
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			SecretKey = secretKey,
			Username = username,
		};

		_ = await client
			.PlayerKickAsync(request, cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<GameTokenReply> GameTokenAsync(
		Uri apiUrl,
		string merchantId,
		string secretKey,
		string gameId,
		string currency,
		string username,
		CancellationToken cancellationToken = default)
	{
		var request = new GameTokenRequest
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			SecretKey = secretKey,
			GameId = gameId,
			Currency = currency,
			Username = username,
		};

		return await client
			.GameTokenAsync(request, cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<GameHistorySettledReply> GameHistorySettledAsync(
		Uri apiUrl,
		string merchantId,
		string secretKey,
		string token,
		CancellationToken cancellationToken = default)
	{
		var request = new GameHistorySettledRequest
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			SecretKey = secretKey,
			Token = token,
		};

		return await client
			.GameHistorySettledAsync(request, cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<GameSessionInfoReply> GameSessionInfoAsync(
		Uri apiUrl,
		string merchantId,
		string secretKey,
		string token,
		CancellationToken cancellationToken = default)
	{
		var request = new GameSessionInfoRequest
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			SecretKey = secretKey,
			Token = token,
		};

		return await client
			.GameSessionInfoAsync(request, cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<GameSessionStatisticsReply> GameSessionStatisticsAsync(
		Uri apiUrl,
		string merchantId,
		string secretKey,
		string token,
		CancellationToken cancellationToken = default)
	{
		var request = new GameSessionStatisticsRequest
		{
			ApiUrl = apiUrl.ToString(),
			MerchantId = merchantId,
			SecretKey = secretKey,
			Token = token,
		};

		return await client
			.GameSessionStatisticsAsync(request, cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}
}
