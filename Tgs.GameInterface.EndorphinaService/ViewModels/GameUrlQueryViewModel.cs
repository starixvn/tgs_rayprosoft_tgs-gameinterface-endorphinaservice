namespace Tgs.GameInterface.EndorphinaService.ViewModels;

public record GameUrlQueryViewModel(
	Uri ApiUrl,
	string MerchantId,
	string SecretKey,
	string GameId,
	string Currency,
	string Username,
	string Language);
