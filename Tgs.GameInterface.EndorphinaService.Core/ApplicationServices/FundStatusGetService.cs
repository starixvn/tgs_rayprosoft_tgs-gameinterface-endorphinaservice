using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class FundStatusGetService(IGameApiClient client) : IGetService<FundStatusQuery, bool>
{
    public ValueTask<bool> GetAsync(FundStatusQuery command, CancellationToken cancellationToken = default)
        => client.SinglePaymentAsync(
            apiUrl: command.ApiUrl,
            merchantId: command.MerchantId,
            secretKey: command.SecretKey,
            transactionId: command.TransactionId,
            cancellationToken: cancellationToken);
}
