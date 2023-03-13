namespace Tgs.GameInterface.EndorphinaService.ViewModels;

public record FundResultViewModel(
    string RefTransactionCode,
    string TransactionId,
    decimal Balance,
    string Response);
