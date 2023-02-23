namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

public record FundStatusQuery(
    Uri ApiUrl,
    string MerchantId,
    string SecretKey,
    string TransactionId);