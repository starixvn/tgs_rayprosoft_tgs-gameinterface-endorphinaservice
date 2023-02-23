namespace Tgs.GameInterface.EndorphinaService.ViewModels;

public record GameSessionInfoQueryViewModel(
    Uri ApiUrl,
    string MerchantId,
    string SecretKey,
    string Token);
