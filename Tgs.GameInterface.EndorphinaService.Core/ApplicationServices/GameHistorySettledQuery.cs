namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;
public record GameHistorySettledQuery(
    Uri ApiUrl,
    string MerchantId,
    string SecretKey,
    string Token);
