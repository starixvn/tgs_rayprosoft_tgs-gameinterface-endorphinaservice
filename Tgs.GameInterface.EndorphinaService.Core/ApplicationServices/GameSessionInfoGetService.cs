using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class GameSessionInfoGetService(IGameApiClient client) : IGetService<GameSessionInfoQuery, GameSessionInfo>
{
    public async ValueTask<GameSessionInfo> GetAsync(GameSessionInfoQuery query, CancellationToken cancellationToken = default)
    {
        var result = await client
            .GameSessionInfoAsync(
            apiUrl: query.ApiUrl,
            merchantId: query.MerchantId,
            secretKey: query.SecretKey,
            token: query.Token,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new GameSessionInfo
        {
            Id = result.Id,
            Username = result.Username,
            GameName = result.GameName,
            Currency = result.Currency,
            AccountType = result.AccountType,
            Created = result.Created,
            Closed = result.Closed,
            ErrorsPending = result.ErrorsPending,
        };
    }
}
