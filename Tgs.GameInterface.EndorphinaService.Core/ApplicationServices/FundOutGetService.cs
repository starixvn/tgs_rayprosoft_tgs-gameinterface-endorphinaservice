using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class FundOutGetService : IGetService<FundOutQuery, FundResult>
{
    private readonly IGameApiClient _client;

    public FundOutGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client.DepositAsync(
            apiUrl: query.ApiUrl,
            merchantId: query.MerchantId,
            secretKey: query.SecretKey,
            username: query.Username,
            refTransactionCode: query.RefTransactionCode,
            amount: query.Amount,
            currency: query.Currency,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return new FundResult(
            RefTransactionCode: result.RefTransactionCode,
            TransactionId: result.TransactionId,
            Balance: result.Balance);
    }
}