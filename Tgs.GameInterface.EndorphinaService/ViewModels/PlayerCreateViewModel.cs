namespace Tgs.GameInterface.EndorphinaService.ViewModels;

public record PlayerCreateViewModel(
    Uri ApiUrl,
    string MerchantId,
    string SecretKey,
    string Username);