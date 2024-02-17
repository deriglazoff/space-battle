namespace SpaceBattle.Api.Commands;

public class BurnFuelCommand(IFuel ship) : ICommand
{
    public void Execute()
    {
        ship.Fuel -= ship.NeedFuel;
    }   
}