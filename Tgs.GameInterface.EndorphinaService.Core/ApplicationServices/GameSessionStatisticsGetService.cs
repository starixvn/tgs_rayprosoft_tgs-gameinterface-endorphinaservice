using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class GameSessionStatisticsGetService : IGetService<GameSessionStatisticsQuery, GameSessionStatisticsResult>
{
    private readonly IGameApiClient _client;

    public GameSessionStatisticsGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameSessionStatisticsResult> GetAsync(GameSessionStatisticsQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client
            .GameSessionStatisticsAsync(
                query.ApiUrl,
                query.MerchantId,
                query.SecretKey,
                query.Token,
                cancellationToken)
            .ConfigureAwait(false);

        return new GameSessionStatisticsResult
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