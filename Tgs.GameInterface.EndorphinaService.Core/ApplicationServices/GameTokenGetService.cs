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
                query.ApiUrl,
                query.MerchantId,
                query.SecretKey,
                query.GameId,
                query.Currency,
                query.Username)
            .ConfigureAwait(false);

        return new GameTokenResult(result!.Token!, result!.Username!, result!.Game!);
    }
}
