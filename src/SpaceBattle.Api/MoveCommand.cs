namespace SpaceBattle.Api;
public class MoveCommand(IMovable ship) : ICommand
{
    public void Execute()
    {
        ship.Position +=ship.Velocity;
    }
}