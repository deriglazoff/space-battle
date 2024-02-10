namespace SpaceBattle.Api;
public class MoveCommand(SpaceShip ship, float dx = 0, float dy = 0) : ICommand
{
    public void Execute()
    {
        ship.x += dx;
        ship.y += dy;
    }
}
