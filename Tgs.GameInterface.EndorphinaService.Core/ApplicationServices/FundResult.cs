namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;

public record FundResult(
    string RefTransactionCode,
    string TransactionId,
    decimal Balance);
