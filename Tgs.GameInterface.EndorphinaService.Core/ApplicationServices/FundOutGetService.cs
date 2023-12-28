using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class FundOutGetService(IGameApiClient client) : IGetService<FundOutQuery, FundResult>
{
    public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
    {
        var result = await client.DepositAsync(
            apiUrl: query.ApiUrl,
            merchantId: query.MerchantId,
            secretKey: query.SecretKey,
            username: query.Username,
            refTransactionCode: query.RefTransactionCode,
            amount: query.Amount,
            currency: query.Currency,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return new FundResult(
            RefTransactionCode: result?.RefTransactionCode ?? string.Empty,
            TransactionId: result?.TransactionId ?? string.Empty,
            Balance: result?.Balance ?? 0M,
            Response: result?.Response ?? string.Empty);
    }
}
