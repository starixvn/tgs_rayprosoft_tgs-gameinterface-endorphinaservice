namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

public record PlayerKickCommand(
    Uri ApiUrl,
    string MerchantId,
    string SecretKey,
    string Username);
