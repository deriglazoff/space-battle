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
        var spaceShip = new Movable(12, 5);

        spaceShip.Move(-7, 3);

        Assert.Equal(5, spaceShip.x);
        Assert.Equal(8, spaceShip.y);
    }

    [Fact(DisplayName = "Ошибка для отрицательного градуса")]
    public void Test()
    {
        var spaceShip = new Rotable();

        Assert.Throws<ValidationException>(() => spaceShip.Rotate(-20));
    }
}