using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class GameSessionInfoGetService : IGetService<GameSessionInfoQuery, GameSessionInfo>
{
    private readonly IGameApiClient _client;

    public GameSessionInfoGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameSessionInfo> GetAsync(GameSessionInfoQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client
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
