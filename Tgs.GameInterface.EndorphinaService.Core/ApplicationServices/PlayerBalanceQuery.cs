namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

public record PlayerBalanceQuery(
    Uri ApiUrl,
    string MerchantId,
    string SecretKey,
    string Username,
    string Currency);