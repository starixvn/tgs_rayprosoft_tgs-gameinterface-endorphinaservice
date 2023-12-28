using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class GameHistorySettledGetService(IGameApiClient client) : IGetService<GameHistorySettledQuery, GameHistorySettledResult>
{
    public async ValueTask<GameHistorySettledResult> GetAsync(GameHistorySettledQuery query, CancellationToken cancellationToken = default)
    {
        var sessionInfo = await client.GameSessionInfoAsync(
            apiUrl: query.ApiUrl,
            merchantId: query.MerchantId,
            secretKey: query.SecretKey,
            token: query.Token,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        if (sessionInfo.Closed is null || sessionInfo.ErrorsPending != 0)
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

        var settleResult = await client.GameSessionStatisticsAsync(
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
