namespace Tgs.GameInterface.EndorphinaService.ViewModels;

public record FundViewModel(
    Uri ApiUrl,
    string MerchantId,
    string SecretKey,
    string Username,
    string Currency,
    string RefTransactionCode,
    decimal Amount);
