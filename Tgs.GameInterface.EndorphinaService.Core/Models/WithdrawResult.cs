namespace Tgs.GameInterface.EndorphinaService.Core.Models;
public record WithdrawResult(
	string RefTransactionCode,
	string TransactionId,
	decimal Balance,
	string Resp);
