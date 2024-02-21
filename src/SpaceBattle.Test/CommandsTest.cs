using NSubstitute;
using SpaceBattle.Api;
using SpaceBattle.Api.Commands;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Xunit;

namespace SpaceBattle.Test;

public class CommandsTest 
{

    [Fact(DisplayName = "Ошибка при недостаточном топливе")]
    public void CheckFuelException()
    {
        var fuelObject= Substitute.For<IFuel>();
        fuelObject.Fuel = 10;
        fuelObject.NeedFuel = 20;
        var command = new CheckFuelCommand(fuelObject);

        Assert.Throws<ValidationException>(command.Execute);
    }

    [Fact(DisplayName = "Успешная проверка топлива")]
    public void CheckFuelTrue()
    {
        var fuelObject = Substitute.For<IFuel>();
        fuelObject.Fuel = 10;
        fuelObject.NeedFuel = 10;
        var command = new CheckFuelCommand(fuelObject);

        command.Execute();
    }

    [Fact(DisplayName = "Успех сжигание топлива")]
    public void BurnFuelCommandTrue()
    {
        var fuelObject = Substitute.For<IFuel>();
        fuelObject.Fuel = 10;
        fuelObject.NeedFuel = 10;
        var command = new BurnFuelCommand(fuelObject);

        command.Execute();

        Assert.Equal(0, fuelObject.Fuel);
    }

    [Fact(DisplayName = "Успех макрокоманды")]
    public void MacroCommandBurnFuel()
    {
        var fuelObject = Substitute.For<IFuel>();
        fuelObject.Fuel = 10;
        fuelObject.NeedFuel = 10;
        var command1 = new CheckFuelCommand(fuelObject);
        var command2 = new BurnFuelCommand(fuelObject);
        var macroCommand = new MacroCommand(command1, command2);


        macroCommand.Execute();

        Assert.Equal(0, fuelObject.Fuel);
    }

    [Fact(DisplayName = "Успех макрокоманды передвижения")]
    public void MacroCommandMove()
    {
        var ship = new SpaceShip
        {
            Fuel = 10,
            NeedFuel = 10,
            Position = new Vector2(12, 5),
            Velocity = new Vector2(-7, 3)
        };

        var command1 = new CheckFuelCommand(ship);
        var command2 = new BurnFuelCommand(ship);
        var command3 = new MoveCommand(ship);
        var macroCommand = new MacroCommand(command1, command2, command3);


        macroCommand.Execute();

        Assert.Equal(0, ship.Fuel);
        Assert.Equal(new Vector2(5, 8), ship.Position);
    }
}