namespace SpaceBattle.Api.Commands;
public class MoveCommand(IMovable ship) : ICommand
{
    public void Execute()
    {
        ship.Position += ship.Velocity;
    }
}