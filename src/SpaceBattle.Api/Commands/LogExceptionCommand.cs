namespace SpaceBattle.Api.Commands;

public interface Ilogger
{
    public void LogError(Exception ex);
}
public class LogExceptionCommand(Ilogger logger, Exception ex) : ICommand
{
    public void Execute()
    {
        logger.LogError(ex);
    }
}
