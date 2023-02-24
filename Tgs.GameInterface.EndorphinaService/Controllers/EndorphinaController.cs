using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;
using Tgs.GameInterface.EndorphinaService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.Controllers;

[ApiController]
[Route("endor")]
public class EndorphinaController : ControllerBase
{
    /// <summary>
    /// Players the create asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://test.endorphina.network",
    ///             "merchantId": "1494",
    ///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
    ///             "username": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("player/create", Name = "PlayerCreate")]
    public ValueTask PlayerCreateAsync(
        [FromServices] ICommandService<PlayerCreateCommand> service,
        PlayerCreateViewModel source)
    {
        var command = new PlayerCreateCommand(
            ApiUrl: source.ApiUrl,
            MerchantId: source.MerchantId,
            SecretKey: source.SecretKey,
            Username: source.Username);

        return service.ExecuteAsync(
                command: command,
                cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Funds the out asynchronous.(Tgs -> Gp)
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://test.endorphina.network",
    ///             "merchantId": "1494",
    ///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
    ///             "username": "4ucmqplxb029",
    ///             "currency": "CNY",
    ///             "refTransactionCode": "a0f49ce0-2c1d-4f50-9fc5-58b610a30bf5",
    ///             "amount": 1000
    ///         }
    /// </remarks>
    [HttpPost("fund/out", Name = "FundOut")]
    public async ValueTask<FundResultViewModel> FundOutAsync(
        [FromServices] IGetService<FundOutQuery, FundResult> service,
        FundViewModel source)
    {
        var query = new FundOutQuery(
            ApiUrl: source.ApiUrl,
            MerchantId: source.MerchantId,
            SecretKey: source.SecretKey,
            Username: source.Username,
            Currency: source.Currency,
            RefTransactionCode: source.RefTransactionCode,
            Amount: source.Amount);

        var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new FundResultViewModel(
            RefTransactionCode: result.RefTransactionCode,
            TransactionId: result.TransactionId,
            Balance: result.Balance);
    }

    /// <summary>
    /// Funds the in asynchronous.(GP -> Tgs)
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://test.endorphina.network",
    ///             "merchantId": "1494",
    ///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
    ///             "username": "4ucmqplxb029",
    ///             "currency": "CNY",
    ///             "refTransactionCode": "a0f49ce0-2c1d-4f50-9fc5-58b610a30bf5",
    ///             "amount": 1000
    ///         }
    /// </remarks>
    [HttpPost("fund/in", Name = "FundIn")]
    public async ValueTask<FundResultViewModel> FundInAsync(
        [FromServices] IGetService<FundInQuery, FundResult> service,
        FundViewModel source)
    {
        var query = new FundInQuery(
            ApiUrl: source.ApiUrl,
            MerchantId: source.MerchantId,
            SecretKey: source.SecretKey,
            Username: source.Username,
            Currency: source.Currency,
            RefTransactionCode: source.RefTransactionCode,
            Amount: source.Amount);

        var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new FundResultViewModel(
            RefTransactionCode: result.RefTransactionCode,
            TransactionId: result.TransactionId,
            Balance: result.Balance);
    }

    /// <summary>
    /// Funds the status asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://test.endorphina.network",
    ///             "merchantId": "1494",
    ///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
    ///             "transactionId": "1676958531344"
    ///         }
    /// </remarks>
    [HttpPost("fund/Status", Name = "FundStatus")]
    public ValueTask<bool> FundStatusAsync(
        [FromServices] IGetService<FundStatusQuery, bool> service,
        FundStatusViewModel source)
    {
        var query = new FundStatusQuery(
            ApiUrl: source.ApiUrl,
            MerchantId: source.MerchantId,
            SecretKey: source.SecretKey,
            TransactionId: source.TransactionId);

        return service.GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the balance asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://test.endorphina.network",
    ///             "merchantId": "1494",
    ///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
    ///             "username": "4ucmqplxb029",
    ///             "currency": "CNY"
    ///         }
    /// </remarks>
    [HttpPost("player/balance", Name = "PlayerBalance")]
    public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
        [FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
        PlayerBalanceViewModel source)
    {
        var query = new PlayerBalanceQuery(
            ApiUrl: source.ApiUrl,
            MerchantId: source.MerchantId,
            SecretKey: source.SecretKey,
            Username: source.Username,
            Currency: source.Currency);

        var result = await service
            .GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new PlayerBalanceResultViewModel(
            result.IsPlayerExist,
            result.Balance);
    }

    /// <summary>
    /// Games the URL asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://test.endorphina.network",
    ///             "merchantId": "1494",
    ///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
    ///             "gameId": "endorphina2_Rabbits",
    ///             "currency": "CNY",
    ///             "username": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("gameurl")]
    public async ValueTask<GameUrlResultViewModel> GameUrlAsync(
        [FromServices] IGetService<GameUrlQuery, string?> service,
        GameUrlQueryViewModel model)
    {
        var result = await service
            .GetAsync(new GameUrlQuery(
                model.ApiUrl,
                model.MerchantId,
                model.SecretKey,
                model.GameId,
                model.Currency,
                model.Username))
            .ConfigureAwait(false);

        return new GameUrlResultViewModel(result);
    }

    /// <summary>
    /// Players the kick asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://test.endorphina.network",
    ///             "merchantId": "1494",
    ///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
    ///             "username": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("player/kick", Name = "PlayerKick")]
    public ValueTask PlayerKickAsync(
        [FromServices] ICommandService<PlayerKickCommand> service,
        PlayerKickViewModel source)
        => service
            .ExecuteAsync(
                command: new PlayerKickCommand(
                    source.ApiUrl,
                    source.MerchantId,
                    source.SecretKey,
                    source.Username),
                cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Games the session statistics asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://test.endorphina.network",
    ///             "merchantId": "1494",
    ///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
    ///             "token": "AA8BB60F5A134264BE838CF6EBA7C8F1"
    ///         }
    /// </remarks>
    [HttpPost("gameSessionStatistics")]
    public async ValueTask<GameSessionStatisticsResultViewModel> GameSessionStatisticsAsync(
        [FromServices] IGetService<GameSessionStatisticsQuery, GameSessionStatisticsInfo> service,
        GameSessionStatisticsQueryViewModel source)
    {
        var result = await service.GetAsync(new GameSessionStatisticsQuery(
            source.ApiUrl,
            source.MerchantId,
            source.SecretKey,
            source.Token))
        .ConfigureAwait(false);

        return new GameSessionStatisticsResultViewModel
        {
            Bet = result.Bet,
            Win = result.Win,
            Games = result.Games,
            JackpotWin = result.JackpotWin,
            Jackpots = result.Jackpots,
            Deposit = result.Deposit,
            Withdraw = result.Withdraw,
            ErrorsPending = result.ErrorsPending,
        };
    }

    /// <summary>
    /// Games the session information asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://test.endorphina.network",
    ///             "merchantId": "1494",
    ///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
    ///             "token": "AA8BB60F5A134264BE838CF6EBA7C8F1"
    ///         }
    /// </remarks>
    [HttpPost("gameSessionInfo")]
    public async ValueTask<GameSessionInfoResultViewModel> GameSessionInfoAsync(
        [FromServices] IGetService<GameSessionInfoQuery, GameSessionInfo> service,
        GameSessionInfoQueryViewModel source)
    {
        var result = await service.GetAsync(new GameSessionInfoQuery(
                source.ApiUrl,
                source.MerchantId,
                source.SecretKey,
                source.Token),
                HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new GameSessionInfoResultViewModel
        {
            Id = result.Id,
            Username = result.Username,
            GameName = result.GameName,
            Currency = result.Currency,
            AccountType = result.AccountType,
            Created = result.Created,
            Closed = result.Closed,
            ErrorsPending = result.ErrorsPending,
        };
    }

    /// <summary>
    /// Games the history settled asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://test.endorphina.network",
    ///             "merchantId": "1494",
    ///             "secretKey": "9BC9E97FFCD74B16B87BD6FA82C8C2AF",
    ///             "token": "AA8BB60F5A134264BE838CF6EBA7C8F1"
    ///         }
    /// </remarks>
    [HttpPost("game/history/settled", Name = "GameHistorySettled")]
    public async ValueTask<GameHistorySettledResultViewModel> GameHistorySettledAsync(
        [FromServices] IGetService<GameHistorySettledQuery, GameHistorySettledResult> service,
        GameHistorySettledViewModel model)
    {
        var query = new GameHistorySettledQuery(
            ApiUrl: model.ApiUrl,
            MerchantId: model.MerchantId,
            SecretKey: model.SecretKey,
            Token: model.Token);

        var gameRecord = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new GameHistorySettledResultViewModel(
                 PlayerCode: gameRecord.PlayerCode,
                 ExternalRoundId: gameRecord.ExternalRoundId,
                 IsClosed: gameRecord.IsClosed,
                 ExternalCreatedOn: gameRecord.ExternalCreatedOn,
                 ExternalClosedOn: gameRecord.ExternalClosedOn,
                 Bet: gameRecord.Bet,
                 ValidBet: gameRecord.ValidBet,
                 PlayerWinLoss: gameRecord.PlayerWinLoss,
                 Turnover: gameRecord.Turnover,
                 Settle: gameRecord.Settle,
                 AdditionalData: gameRecord.AdditionalData,
                 ExternalGameId: gameRecord.ExternalGameId);
    }
}
