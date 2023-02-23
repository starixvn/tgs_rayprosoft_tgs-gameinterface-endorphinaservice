using Tgs.GameInterface.EndorphinaService.Core.Models;

namespace Tgs.GameInterface.EndorphinaService.Core;

public interface IGameApiClient
{
    /// <summary>
    /// 註冊玩家
    /// </summary>
    /// <param name="apiUrl">The API URL.</param>
    /// <param name="merchantId">The merchant identifier.</param>
    /// <param name="secretKey">The secret key.</param>
    /// <param name="username">The username.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    ValueTask PlayerRegistrationAsync(
        Uri apiUrl,
        string merchantId,
        string secretKey,
        string username,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 查詢玩家幣別和餘額
    /// </summary>
    /// <param name="apiUrl">The API URL.</param>
    /// <param name="merchantId">The merchant identifier.</param>
    /// <param name="secretKey">The secret key.</param>
    /// <param name="username">The username.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    IAsyncEnumerable<PlayerAccountsInfoResult?> PlayerAccountsInfoAsync(
        Uri apiUrl,
        string merchantId,
        string secretKey,
        string username,
        CancellationToken cancellationToken = default);

    ValueTask<DepositResult?> DepositAsync(
        Uri apiUrl,
        string merchantId,
        string secretKey,
        string username,
        string refTransactionCode,
        decimal amount,
        string currency,
        CancellationToken cancellationToken = default);

    ValueTask<WithdrawResult?> WithdrawAsync(
        Uri apiUrl,
        string merchantId,
        string secretKey,
        string username,
        string refTransactionCode,
        decimal amount,
        string currency,
        CancellationToken cancellationToken = default);

    ValueTask<bool> SinglePaymentAsync(
        Uri apiUrl,
        string merchantId,
        string secretKey,
        string transactionId,
        CancellationToken cancellationToken = default);

    ValueTask<RequestSessionTokenResult?> RequestSessionTokenAsync(
        Uri apiUrl,
        string merchantId,
        string secretKey,
        string providerId,
        string gameId,
        string currency,
        string username,
        CancellationToken cancellationToken = default);

    ValueTask<bool> KickPlayerAsync(
        Uri apiUrl,
        string merchantId,
        string secretKey,
        string username,
        CancellationToken cancellationToken = default);

    ValueTask<GameSessionStatisticsResult?> GameSessionStatisticsAsync(
        Uri apiUrl,
        string merchantId,
        string secretKey,
        string token,
        CancellationToken cancellationToken = default);

    ValueTask<GameSessionInfoResult?> GameSessionInfoAsync(
        Uri apiUrl,
        string merchantId,
        string secretKey,
        string token,
        CancellationToken cancellationToken = default);
}
