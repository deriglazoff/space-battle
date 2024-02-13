using System.Numerics;

namespace SpaceBattle.Api;

public interface IMovable
{
    public Vector2 Position { get; set; }

    public Vector2 Velocity { get; set; }
}
public interface IRotation
{
    public int Direction { get; set; }
    public int AngularVelocity { get; }
}
public class SpaceShip : IMovable, IRotation
{
    public int Direction { get; set; }

    public Vector2 Position { get; set; }

    public Vector2 Velocity { get; set; }

    public int AngularVelocity { get; }
}