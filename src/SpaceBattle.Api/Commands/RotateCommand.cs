using System.ComponentModel.DataAnnotations;

namespace SpaceBattle.Api.Commands;

public class RotateCommand(IRotation ship) : ICommand
{
    public void Execute()
    {
        if (ship.Direction > 360 || ship.Direction < 0)
            throw new ValidationException();
        ship.Direction += ship.AngularVelocity % ship.Direction;
    }
}