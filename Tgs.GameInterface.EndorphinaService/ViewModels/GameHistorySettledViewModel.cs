namespace Tgs.GameInterface.EndorphinaService.ViewModels;

public record GameHistorySettledViewModel(
    Uri ApiUrl,
    string MerchantId,
    string SecretKey,
    string Token);
