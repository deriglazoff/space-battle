namespace SpaceBattle.Api.Commands;

public class RetryCommand(ICommand command) : ICommand
{
    public void Execute()
    {
        command.Execute();
    }
}
