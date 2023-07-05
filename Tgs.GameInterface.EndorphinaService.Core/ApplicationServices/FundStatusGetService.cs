using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

internal class FundStatusGetService : IGetService<FundStatusQuery, bool>
{
    private readonly IGameApiClient _client;

    public FundStatusGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<bool> GetAsync(FundStatusQuery command, CancellationToken cancellationToken = default)
        => _client.SinglePaymentAsync(
            apiUrl: command.ApiUrl,
            merchantId: command.MerchantId,
            secretKey: command.SecretKey,
            transactionId: command.TransactionId,
            cancellationToken: cancellationToken);
}
