namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;
public record GameSessionInfoQuery(
    Uri ApiUrl,
    string MerchantId,
    string SecretKey,
    string Token);