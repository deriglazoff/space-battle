using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.Core.Arguments;
using SpaceBattle.Api;
using SpaceBattle.Api.Commands;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace SpaceBattle.Test;

public class ExceptionTest
{
    [Fact(DisplayName = "Реализовать обработчик исключения, который ставит Команду, пишущую в лог в очередь Команд.")]
    public async Task LogExceptionCommand()
    {
        ExceptionHandler exceptionHandler = new ExceptionHandler();

        var logger = Substitute.For<Ilogger>();

        exceptionHandler.RegisterHandler(typeof(RotateCommand), typeof(ValidationException),
            (c, e) => new LogExceptionCommand(logger, e));

        var ship = new SpaceShip { Direction = 0 };
        var command = new RotateCommand(ship, -20);

        var server = new Server([command], exceptionHandler);
        var token = new CancellationTokenSource();


        await server.StartAsync(token.Token);


        logger.Received().LogError(Arg.Any<ValidationException>());
    }

    [Fact(DisplayName = "Реализовать обработчик исключения, который ставит в очередь Команду - повторитель команды, выбросившей исключение.")]
    public async Task RetryCommand()
    {
        ExceptionHandler exceptionHandler = new ExceptionHandler();

        var commandException = Substitute.For<ICommand>();
        commandException.When(fake => fake.Execute()).Do(call => { throw new Exception(); });
        exceptionHandler.RegisterHandler(commandException.GetType(), typeof(Exception),
            (c, e) => new RetryCommand(commandException));

        var server = new Server([commandException], exceptionHandler);
        var token = new CancellationTokenSource();


        await server.StartAsync(token.Token);


        commandException.Received(2).Execute();
    }


    [Fact(DisplayName = "С помощью Команд из пункта 4 и пункта 6 реализовать следующую обработку исключений:" +
        "при первом выбросе исключения повторить команду, при повторном выбросе исключения записать информацию в лог.")]
    public async Task DoubleRetryCommand()
    {
        ExceptionHandler exceptionHandler = new ExceptionHandler();

        var logger = Substitute.For<Ilogger>();
        var commandException = Substitute.For<ICommand>();
        commandException.When(fake => fake.Execute()).Do(call => { throw new Exception(); });
        exceptionHandler.RegisterHandler(commandException.GetType(), typeof(Exception),
            (c, e) => new RetryCommand(commandException));
        exceptionHandler.RegisterHandler(typeof(RetryCommand), typeof(Exception),
            (c, e) => new LogExceptionCommand(logger, e));

        var server = new Server([commandException], exceptionHandler);
        var token = new CancellationTokenSource();


        await server.StartAsync(token.Token);


        commandException.Received(2).Execute();
        logger.Received().LogError(Arg.Any<Exception>());
    }
}