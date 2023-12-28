using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class PlayerBalanceGetService(IGameApiClient client) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
    {
        var result = await client
            .PlayerAccountsInfoAsync(
                apiUrl: query.ApiUrl,
                merchantId: query.MerchantId,
                secretKey: query.SecretKey,
                username: query.Username,
                currency: query.Currency,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return result;
    }
}
