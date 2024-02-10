using SpaceBattle.Api;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace SpaceBattle.Test;

public class SpaceShipTest
{
    [Fact(DisplayName = "Для объекта, находящегося в точке (12, 5) " +
        "и движущегося со скоростью (-7, 3) " +
        "движение меняет положение объекта на (5, 8)")]
    public void Move_Equal()
    {
        var ship = new SpaceShip { x = 12, y = 5 };
        var command = new MoveCommand(ship, -7, 3);

        command.Execute();

        Assert.Equal(5, ship.x);
        Assert.Equal(8, ship.y);
    }

    [Fact(DisplayName = "Ошибка для отрицательного градуса")]
    public void Test()
    {
        var ship = new SpaceShip { Direction = 0 };
        var command = new RotateCommand(ship, -20);

        Assert.Throws<ValidationException>(() => command.Execute());
    }
}