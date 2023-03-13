namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

public record GameUrlQuery(
    Uri ApiUrl,
    string MerchantId,
    string SecretKey,
    string GameId,
    string Currency,
    string Username,
    string Language);
