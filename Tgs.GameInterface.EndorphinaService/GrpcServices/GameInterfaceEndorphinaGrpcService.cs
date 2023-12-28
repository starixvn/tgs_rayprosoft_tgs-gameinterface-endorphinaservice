using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;
using Tgs.GameInterface.EndorphinaService.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.GrpcServices;

internal class GameInterfaceEndorphinaGrpcService(
    ILogger<GameInterfaceEndorphinaGrpcService> logger,
    ICommandService<PlayerCreateCommand> playerCreateService,
    IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
    ICommandService<PlayerKickCommand> playerKickService,
    IGetService<FundInQuery, FundResult> fundInService,
    IGetService<FundOutQuery, FundResult> fundOutService,
    IGetService<FundStatusQuery, bool> fundStatusService,
    IGetService<GameUrlQuery, string?> gameUrlService,
    IGetService<GameTokenQuery, GameTokenResult> gameTokenService,
    IGetService<GameHistorySettledQuery, GameHistorySettledResult> gameHistoryService,
    IGetService<GameSessionInfoQuery, GameSessionInfo> gameSessionInfoService,
    IGetService<GameSessionStatisticsQuery, GameSessionStatisticsInfo> gameSessionStatisticsService) : GameInterfaceEndorphinaService.GameInterfaceEndorphinaServiceBase
{
    public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
    {
        logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

        return Task.FromResult(new CheckingServiceReply
        {
            MachineName = Environment.MachineName
        });
    }

    public override async Task<BaseReply> PlayerCreate(PlayerCreateRequest request, ServerCallContext context)
    {
        var command = new PlayerCreateCommand(
           ApiUrl: new(request.ApiUrl),
           MerchantId: request.MerchantId,
           SecretKey: request.SecretKey,
           Username: request.Username);

        await playerCreateService
            .ExecuteAsync(
                command: command,
                cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = true
        };
    }

    public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
    {
        var query = new PlayerBalanceQuery(
           ApiUrl: new(request.ApiUrl),
           MerchantId: request.MerchantId,
           SecretKey: request.SecretKey,
           Username: request.Username,
           Currency: request.Currency);

        var result = await playerBalanceService
            .GetAsync(
                query: query,
                cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceReply()
        {
            Balance = result.Balance,
            IsPlayerExist = result.IsPlayerExist,
        };
    }

    public override async Task<BaseReply> PlayerKick(PlayerKickRequest request, ServerCallContext context)
    {
        var command = new PlayerKickCommand(
            ApiUrl: new(request.ApiUrl),
            MerchantId: request.MerchantId,
            SecretKey: request.SecretKey,
            Username: request.Username);

        await playerKickService
            .ExecuteAsync(
                command,
                cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = true
        };
    }

    public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
    {
        var query = new FundInQuery(
            ApiUrl: new(request.ApiUrl),
            MerchantId: request.MerchantId,
            SecretKey: request.SecretKey,
            Username: request.Username,
            Currency: request.Currency,
            RefTransactionCode: request.RefTransactionCode,
            Amount: request.Amount);

        var result = await fundInService
            .GetAsync(query, cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new FundReply()
        {
            Balance = result.Balance,
            RefTransactionCode = result.RefTransactionCode,
            TransactionId = result.TransactionId,
            Response = result.Response,
        };
    }

    public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
    {
        var query = new FundOutQuery(
            ApiUrl: new(request.ApiUrl),
            MerchantId: request.MerchantId,
            SecretKey: request.SecretKey,
            Username: request.Username,
            Currency: request.Currency,
            RefTransactionCode: request.RefTransactionCode,
            Amount: request.Amount);

        var result = await fundOutService
            .GetAsync(query, cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new FundReply()
        {
            Balance = result.Balance,
            RefTransactionCode = result.RefTransactionCode,
            TransactionId = result.TransactionId,
            Response = result.Response,
        };
    }

    public override async Task<FundStatusReply> FundStatus(FundStatusRequest request, ServerCallContext context)
    {
        var query = new FundStatusQuery(
            ApiUrl: new(request.ApiUrl),
            MerchantId: request.MerchantId,
            SecretKey: request.SecretKey,
            TransactionId: request.TransactionId);

        var result = await fundStatusService
            .GetAsync(query, cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new FundStatusReply()
        {
            IsOrderExist = result
        };
    }

    public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
    {
        var query = new GameUrlQuery(
            ApiUrl: new(request.ApiUrl),
            MerchantId: request.MerchantId,
            SecretKey: request.SecretKey,
            GameId: request.GameId,
            Currency: request.Currency,
            Username: request.Username,
            Language: request.Language);

        var result = await gameUrlService
            .GetAsync(query, cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new GameUrlReply()
        {
            GameUrl = result
        };
    }

    public override async Task<GameTokenReply> GameToken(GameTokenRequest request, ServerCallContext context)
    {
        var query = new GameTokenQuery(
            ApiUrl: new(request.ApiUrl),
            MerchantId: request.MerchantId,
            SecretKey: request.SecretKey,
            GameId: request.GameId,
            Currency: request.Currency,
            Username: request.Username);

        var result = await gameTokenService
            .GetAsync(query, cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new GameTokenReply()
        {
            Token = result.Token,
            Game = result.Game,
            Username = result.Username
        };
    }

    public override async Task<GameHistorySettledReply> GameHistorySettled(GameHistorySettledRequest request, ServerCallContext context)
    {
        var query = new GameHistorySettledQuery(
            ApiUrl: new Uri(request.ApiUrl),
            MerchantId: request.MerchantId,
            SecretKey: request.SecretKey,
            Token: request.Token);

        var result = await gameHistoryService
            .GetAsync(query, cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new GameHistorySettledReply()
        {
            Bet = result.Bet,
            ValidBet = result.ValidBet,
            Settle = result.Settle,
            Turnover = result.Turnover,
            PlayerWinLoss = result.PlayerWinLoss,
            IsClosed = result.IsClosed,
            ExternalGameId = result.ExternalGameId,
            ExternalRoundId = result.ExternalRoundId,
            ExternalCreatedOn = result.ExternalCreatedOn.ToTimestamp(),
            ExternalClosedOn = result.ExternalClosedOn == null
                ? DateTimeOffset.UnixEpoch.ToTimestamp()
                : result.ExternalClosedOn.Value.ToTimestamp(),
            PlayerCode = result.PlayerCode,
            AdditionalData = result.AdditionalData,
        };
    }

    public override async Task<GameSessionInfoReply> GameSessionInfo(GameSessionInfoRequest request, ServerCallContext context)
    {
        var query = new GameSessionInfoQuery(
            ApiUrl: new Uri(request.ApiUrl),
            MerchantId: request.MerchantId,
            SecretKey: request.SecretKey,
            Token: request.Token);

        var result = await gameSessionInfoService
            .GetAsync(query, cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new GameSessionInfoReply()
        {
            Id = result.Id,
            Username = result.Username,
            GameName = result.GameName,
            Currency = result.Currency,
            AccountType = result.AccountType,
            Created = result.Created.ToTimestamp(),
            Closed = result.Closed == null
                ? DateTimeOffset.UnixEpoch.ToTimestamp()
                : result.Closed.Value.ToTimestamp(),
            ErrorsPending = result.ErrorsPending,
        };
    }

    public override async Task<GameSessionStatisticsReply> GameSessionStatistics(GameSessionStatisticsRequest request, ServerCallContext context)
    {
        var query = new GameSessionStatisticsQuery(
            ApiUrl: new Uri(request.ApiUrl),
            MerchantId: request.MerchantId,
            SecretKey: request.SecretKey,
            Token: request.Token);

        var result = await gameSessionStatisticsService
            .GetAsync(query, cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new GameSessionStatisticsReply()
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
}
