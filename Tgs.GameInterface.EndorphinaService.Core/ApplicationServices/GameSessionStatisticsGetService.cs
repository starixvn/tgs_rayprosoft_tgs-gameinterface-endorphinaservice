using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class GameSessionStatisticsGetService(IGameApiClient client) : IGetService<GameSessionStatisticsQuery, GameSessionStatisticsInfo>
{
	public async ValueTask<GameSessionStatisticsInfo> GetAsync(GameSessionStatisticsQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client
			.GameSessionStatisticsAsync(
				query.ApiUrl,
				query.MerchantId,
				query.SecretKey,
				query.Token,
				cancellationToken)
			.ConfigureAwait(false);

		return new GameSessionStatisticsInfo
		{
			Bet = result.Bet,
			Win = result.Win,
			Games = result.Games,
			JackpotWin = result.JackpotWin,
			Jackpots = result.Jackpots,
			Deposit = result.Deposit,
			Withdraw = result.Withdraw,
			ErrorsPending = result.ErrorsPending,
		};
	}
}
