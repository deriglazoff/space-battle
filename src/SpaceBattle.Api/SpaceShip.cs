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

public interface IFuel
{
    public int Fuel { get; set; }

    public int NeedFuel { get; set; }
}
public class SpaceShip : IMovable, IRotation, IFuel
{
    public int Direction { get; set; }

    public Vector2 Position { get; set; }

    public Vector2 Velocity { get; set; }

    public int AngularVelocity { get; }

    public int Fuel { get; set; }

    public int NeedFuel { get; set; }
}