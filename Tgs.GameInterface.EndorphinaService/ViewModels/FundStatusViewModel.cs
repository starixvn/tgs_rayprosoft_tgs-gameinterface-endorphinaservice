namespace Tgs.GameInterface.EndorphinaService.ViewModels;

public record FundStatusViewModel(
	Uri ApiUrl,
	string MerchantId,
	string SecretKey,
	string TransactionId);
