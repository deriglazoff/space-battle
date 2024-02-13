using SpaceBattle.Api;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Xunit;

namespace SpaceBattle.Test;

public class SpaceShipTest
{
    [Fact(DisplayName = "Для объекта, находящегося в точке (12, 5) " +
        "и движущегося со скоростью (-7, 3) " +
        "движение меняет положение объекта на (5, 8)")]
    public void Move_Equal()
    {
        var ship = new SpaceShip { Position = new Vector2(12, 5), Velocity = new Vector2(-7, 3) };
        var command = new MoveCommand(ship);

        command.Execute();

        Assert.Equal(new Vector2(5, 8), ship.Position);
    }

    [Fact(DisplayName = "Ошибка для отрицательного градуса")]
    public void Test()
    {
        var ship = new SpaceShip { Direction = -20 };
        var command = new RotateCommand(ship);

        Assert.Throws<ValidationException>(command.Execute);
    }
}