namespace Tgs.GameInterface.EndorphinaService.ViewModels;

public record PlayerBalanceViewModel(
    Uri ApiUrl,
    string MerchantId,
    string SecretKey,
    string Username,
    string Currency);
