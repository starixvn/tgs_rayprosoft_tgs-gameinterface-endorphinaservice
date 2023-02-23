using Grpc.Core;
using Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;
using Tgs.GameInterface.EndorphinaService.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.EndorphinaService.GrpcServices;

internal class GameInterfaceEndorphinaGrpcService : GameInterfaceEndorphinaService.GameInterfaceEndorphinaServiceBase
{
    private readonly ICommandService<PlayerCreateCommand> _playerCreateService;
    private readonly IGetService<PlayerBalanceQuery, PlayerBalanceResult> _playerBalanceService;
    private readonly ICommandService<PlayerKickCommand> _playerKickService;
    private readonly IGetService<FundInQuery, FundResult> _fundInService;
    private readonly IGetService<FundOutQuery, FundResult> _fundOutService;
    private readonly IGetService<FundStatusQuery, bool> _fundStatusService;
    private readonly IGetService<GameUrlQuery, string?> _gameUrlService;

    public GameInterfaceEndorphinaGrpcService(
        ICommandService<PlayerCreateCommand> playerCreateService,
        IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
        ICommandService<PlayerKickCommand> playerKickService,
        IGetService<FundInQuery, FundResult> fundInService,
        IGetService<FundOutQuery, FundResult> fundOutService,
        IGetService<FundStatusQuery, bool> fundStatusService,
        IGetService<GameUrlQuery, string?> gameUrlService)
    {
        _playerCreateService = playerCreateService ?? throw new ArgumentNullException(nameof(playerCreateService));
        _playerBalanceService = playerBalanceService ?? throw new ArgumentNullException(nameof(playerBalanceService));
        _playerKickService = playerKickService ?? throw new ArgumentNullException(nameof(playerKickService));
        _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
        _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
        _fundStatusService = fundStatusService ?? throw new ArgumentNullException(nameof(fundStatusService));
        _gameUrlService = gameUrlService ?? throw new ArgumentNullException(nameof(gameUrlService));
    }

    public override async Task<BaseReply> PlayerCreate(PlayerCreateRequest request, ServerCallContext context)
    {
        var command = new PlayerCreateCommand(
           ApiUrl: new(request.ApiUrl),
           MerchantId: request.MerchantId,
           SecretKey: request.SecretKey,
           Username: request.Username);

        await _playerCreateService
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

        var result = await _playerBalanceService
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

        await _playerKickService
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

        var result = await _fundInService
            .GetAsync(query, cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new FundReply()
        {
            Balance = result.Balance,
            RefTransactionCode = result.RefTransactionCode,
            TransactionId = result.TransactionId,
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

        var result = await _fundOutService
            .GetAsync(query, cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new FundReply()
        {
            Balance = result.Balance,
            RefTransactionCode = result.RefTransactionCode,
            TransactionId = result.TransactionId,
        };
    }

    public override async Task<FundStatusReply> FundStatus(FundStatusRequest request, ServerCallContext context)
    {
        var query = new FundStatusQuery(
            ApiUrl: new(request.ApiUrl),
            MerchantId: request.MerchantId,
            SecretKey: request.SecretKey,
            TransactionId: request.TransactionId);

        var result = await _fundStatusService
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
            Username: request.Username);

        var result = await _gameUrlService
            .GetAsync(query, cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new GameUrlReply()
        {
            GameUrl = result
        };
    }
}
