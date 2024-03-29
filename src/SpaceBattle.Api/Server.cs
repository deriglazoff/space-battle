﻿using System.Collections.Concurrent;

namespace SpaceBattle.Api;

public class Server(BlockingCollection<ICommand> commands, ExceptionHandler handler) : IHostedService
{
    public BlockingCollection<ICommand> Commands = commands;

    public ExceptionHandler _handler = handler;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (commands.Any() is false)
            {
                break;
            }
            var command = Commands.Take(cancellationToken);
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
        throw new NotImplementedException();
    }
}
