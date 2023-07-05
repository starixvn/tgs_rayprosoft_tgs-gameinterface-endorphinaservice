using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class GameTokenGetService : IGetService<GameTokenQuery, GameTokenResult>
{
    private readonly IGameApiClient _client;

    public GameTokenGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameTokenResult> GetAsync(GameTokenQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client
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
