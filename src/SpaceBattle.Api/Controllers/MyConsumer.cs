using MassTransit;
using SpaceBattle.Api.Commands;

namespace SpaceBattle.Api.Controllers;

public class MyConsumer : IConsumer<MyMessage>
{
    public Task Consume(ConsumeContext<MyMessage> context)
    {
        var command = new InterpretCommand(context.Message);
        command.Execute();
        return Task.CompletedTask;
    }



}
public class MyMessage
{
    public Guid OperationId { get; set; }
    public Guid GameId { get; set; }

    public Guid ObjectId { get; set; }

    public object Argumets { get; set; }
}