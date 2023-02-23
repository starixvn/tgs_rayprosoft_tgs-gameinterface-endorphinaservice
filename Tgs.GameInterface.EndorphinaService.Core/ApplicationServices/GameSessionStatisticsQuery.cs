namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;
public record GameSessionStatisticsQuery(
    Uri ApiUrl,
    string MerchantId,
    string SecretKey,
    string Token);