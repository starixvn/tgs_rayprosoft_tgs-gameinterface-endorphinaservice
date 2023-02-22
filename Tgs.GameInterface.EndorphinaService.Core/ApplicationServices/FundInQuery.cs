namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

public record FundInQuery(
    Uri ApiUrl,
    string MerchantId,
    string SecretKey,
    string Username,
    string Currency,
    string RefTransactionCode,
    decimal Amount);
