namespace Tgs.GameInterface.EndorphinaService.ViewModels;

public record PlayerKickViewModel(
    Uri ApiUrl,
    string MerchantId,
    string SecretKey,
    string Username);