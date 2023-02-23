using Tgs.GameInterface.EndorphinaService.Protos;

namespace Tgs.GameInterface.EndorphinaService.Client;

public interface IGameInterfaceEndorphinaClient
{
    ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string merchantId,
        string secretKey,
        string username,
        CancellationToken cancellationToken = default);

    ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string merchantId,
        string secretKey,
        string username,
        string currency,
        CancellationToken cancellationToken = default);

    ValueTask PlayerKickAsync(
        Uri apiUrl,
        string merchantId,
        string secretKey,
        string username,
        CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundInAsync(
        Uri apiUrl,
        string merchantId,
        string secretKey,
        string username,
        string currency,
        string refTransactionCode,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundOutAsync(
        Uri apiUrl,
        string merchantId,
        string secretKey,
        string username,
        string currency,
        string refTransactionCode,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<FundStatusReply> FundStatusAsync(
        Uri apiUrl,
        string merchantId,
        string secretKey,
        string transactionId,
        CancellationToken cancellationToken = default);

    ValueTask<GameUrlReply> GameUrlAsync(
        Uri apiUrl,
        string merchantId,
        string secretKey,
        string providerId,
        string gameId,
        string currency,
        string username,
        CancellationToken cancellationToken = default);
}