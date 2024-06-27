namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

public record PlayerLogoutCommand(
	Uri ApiUrl,
	string MerchantId,
	string SecretKey,
	string Username);
