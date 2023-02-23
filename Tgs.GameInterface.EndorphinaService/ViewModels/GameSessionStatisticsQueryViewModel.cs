namespace Tgs.GameInterface.EndorphinaService.ViewModels;

public record GameSessionStatisticsQueryViewModel(
    Uri ApiUrl,
    string MerchantId,
    string SecretKey,
    string Token);