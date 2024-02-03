using System.ComponentModel.DataAnnotations;

namespace SpaceBattle.Api;

public interface IRotable
{
    public int GetDirection();
    public void Rotate(int direction);
}

public class Rotable : IRotable
{
    public int Direction = 0;
    public int GetDirection() => Direction;
    public void Rotate(int direction)
    {
        if (direction > 360 || direction < 0)
            throw new ValidationException();
        Direction = direction;
    }
}