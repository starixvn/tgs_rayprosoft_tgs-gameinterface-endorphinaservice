using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class GameSessionInfoGetService : IGetService<GameSessionInfoQuery, GameSessionInfoResult>
{
    private readonly IGameApiClient _client;

    public GameSessionInfoGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameSessionInfoResult> GetAsync(GameSessionInfoQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client.GameSessionInfoAsync(
            query.ApiUrl,
            query.MerchantId,
            query.SecretKey,
            query.Token).ConfigureAwait(false);

        return new GameSessionInfoResult
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