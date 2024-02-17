using System.ComponentModel.DataAnnotations;

namespace SpaceBattle.Api.Commands;

public class CheckFuelCommand(IFuel ship) : ICommand
{
    public void Execute()
    {
        if (ship.Fuel < ship.NeedFuel)
            throw new ValidationException();
    }
}
