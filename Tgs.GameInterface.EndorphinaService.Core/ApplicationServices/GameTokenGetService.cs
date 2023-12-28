using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class GameTokenGetService(IGameApiClient client) : IGetService<GameTokenQuery, GameTokenResult>
{
    public async ValueTask<GameTokenResult> GetAsync(GameTokenQuery query, CancellationToken cancellationToken = default)
    {
        var result = await client
            .RequestSessionTokenAsync(
                apiUrl: query.ApiUrl,
                merchantId: query.MerchantId,
                secretKey: query.SecretKey,
                gameId: query.GameId,
                currency: query.Currency,
                username: query.Username,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new GameTokenResult(result!.Token!, result!.Username!, result!.Game!);
    }
}
