using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class PlayerBalanceGetService : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    private readonly IGameApiClient _client;

    public PlayerBalanceGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client
            .PlayerAccountsInfoAsync(
                apiUrl: query.ApiUrl,
                merchantId: query.MerchantId,
                secretKey: query.SecretKey,
                username: query.Username,
                cancellationToken: cancellationToken)
            .ToArrayAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (result is null)
            return new PlayerBalanceResult(false, null);

        if (result.Length == 0)
            return new PlayerBalanceResult(true, null);

        var accountInfo = result.FirstOrDefault(x => x.Currency == query.Currency);

        if (accountInfo == null)
            return new PlayerBalanceResult(true, null);

        return new PlayerBalanceResult(
             IsPlayerExist: true,
             Balance: accountInfo.Balance);
    }
}