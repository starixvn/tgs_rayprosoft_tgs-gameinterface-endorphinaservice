using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class GameHistorySettledGetService : IGetService<GameHistorySettledQuery, GameHistorySettledResult>
{
    private readonly IGameApiClient _client;

    public GameHistorySettledGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameHistorySettledResult> GetAsync(GameHistorySettledQuery query, CancellationToken cancellationToken = default)
    {
        var sessionInfo = await _client.GameSessionInfoAsync(
            apiUrl: query.ApiUrl,
            merchantId: query.MerchantId,
            secretKey: query.SecretKey,
            token: query.Token,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        if (sessionInfo.Closed is null)
            return new GameHistorySettledResult(
                PlayerCode: sessionInfo.Username,
                ExternalRoundId: sessionInfo.Id,
                IsClosed: false,
                ExternalCreatedOn: sessionInfo.Created,
                ExternalClosedOn: null,
                Bet: 0M,
                ValidBet: 0M,
                PlayerWinLoss: 0M,
                Turnover: 0M,
                Settle: 0M,
                AdditionalData: string.Empty,
                ExternalGameId: sessionInfo.GameName);

        var settleResult = await _client.GameSessionStatisticsAsync(
                apiUrl: query.ApiUrl,
                merchantId: query.MerchantId,
                secretKey: query.SecretKey,
                token: query.Token,
                cancellationToken: cancellationToken).ConfigureAwait(false);

        var playerWinLoss = settleResult.Win - settleResult.Bet;

        return new GameHistorySettledResult(
            PlayerCode: sessionInfo.Username,
            ExternalRoundId: sessionInfo.Id,
            IsClosed: true,
            ExternalCreatedOn: sessionInfo.Created,
            ExternalClosedOn: sessionInfo.Closed.Value,
            Bet: settleResult.Bet,
            ValidBet: settleResult.Bet,
            PlayerWinLoss: playerWinLoss,
            Turnover: settleResult.Bet,
            Settle: settleResult.Win,
            AdditionalData: string.Empty,
            ExternalGameId: sessionInfo.GameName);
    }
}
