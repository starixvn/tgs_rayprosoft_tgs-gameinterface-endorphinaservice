namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;
public record GameTokenQuery(
	Uri ApiUrl,
	string MerchantId,
	string SecretKey,
	string GameId,
	string Currency,
	string Username);
