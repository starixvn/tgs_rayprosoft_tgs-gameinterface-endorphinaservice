using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class GameUrlGetService : IGetService<GameUrlQuery, string?>
{
    private readonly IGameApiClient _client;

    public GameUrlGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<string?> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client.RequestSessionTokenAsync(
                query.ApiUrl,
                query.MerchantId,
                query.SecretKey,
                query.GameId,
                query.Currency,
                query.Username,
                cancellationToken).ConfigureAwait(false);

        var token = result.Token;

        var sign = EndorphinaHasher.ComputeHash($"{token}{query.SecretKey}");

        return $"{query.ApiUrl}/icasino/sessions/launch?token={token}&sign={sign}";
    }
}
