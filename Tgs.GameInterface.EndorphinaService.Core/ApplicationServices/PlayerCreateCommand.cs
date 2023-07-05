namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;
public record PlayerCreateCommand(
    Uri ApiUrl,
    string MerchantId,
    string SecretKey,
    string Username);
