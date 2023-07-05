namespace Tgs.GameInterface.EndorphinaService.Core.ApplicationServices;
public record GameSessionStatisticsInfo
{
    public decimal Bet { get; init; }
    public decimal Win { get; init; }
    public int Games { get; init; }
    public decimal JackpotWin { get; init; }
    public int Jackpots { get; init; }

    public decimal Deposit { get; init; }
    public decimal Withdraw { get; init; }
    public int ErrorsPending { get; init; }
};
