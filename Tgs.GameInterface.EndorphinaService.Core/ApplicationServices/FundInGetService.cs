using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class FundInGetService : IGetService<FundInQuery, FundResult>
{
    private readonly IGameApiClient _client;

    public FundInGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client.WithdrawAsync(
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
