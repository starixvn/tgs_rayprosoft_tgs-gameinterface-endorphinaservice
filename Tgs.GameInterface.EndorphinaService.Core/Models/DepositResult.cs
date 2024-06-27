namespace Tgs.GameInterface.EndorphinaService.Core.Models;
public record DepositResult(
	string RefTransactionCode,
	string TransactionId,
	decimal Balance,
	string Resp);
