using SpaceBattle.Api.Controllers;

namespace SpaceBattle.Api.Commands;

public class InterpretCommand(MyMessage message) : ICommand
{
    public void Execute()
    {
        //throw new NotImplementedException();
    }
}
