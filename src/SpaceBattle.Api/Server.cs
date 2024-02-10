using System.Collections.Concurrent;

namespace SpaceBattle.Api;

public class Server(BlockingCollection<ICommand> commands, ExceptionHandler handler) : IHostedService
{
    public BlockingCollection<ICommand> Commands = commands;

    public ExceptionHandler _handler = handler;

    private bool stop = false;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        while (!stop)
        {
            if (Commands.Any() is false)
            {
                break;
            }
            var command = Commands.Take();
            try
            {
                command.Execute();
            }
            catch (Exception ex)
            {
                var cmdEx = _handler.Handle(command, ex);
                Commands.Add(cmdEx);
            }

        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        stop = true;
        return Task.CompletedTask;
    }
}
