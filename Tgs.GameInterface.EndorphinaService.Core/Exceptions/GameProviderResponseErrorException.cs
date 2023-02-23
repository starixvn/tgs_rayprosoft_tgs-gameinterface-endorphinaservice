namespace Tgs.GameInterface.EndorphinaService.Core.Exceptions;

public class GameProviderResponseErrorException : Exception
{
    public GameProviderResponseErrorException(string responseMessage) : base(
       $"The game manufacturer responds with an error code, please contact the engineer or game supplier for assistance." +
       $"({responseMessage})")
    {
    }
}
