using System.ComponentModel.DataAnnotations;

namespace SpaceBattle.Api;

public class RotateCommand(SpaceShip ship, int direction) : ICommand
{
    public void Execute()
    {
        if (direction > 360 || direction < 0)
            throw new ValidationException();
        ship.Direction = direction;
    }
}