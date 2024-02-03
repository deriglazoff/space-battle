namespace SpaceBattle.Api;

public interface IMoveble
{
    public void Move(int dx, int dy);
}
public class Movable(float x = 0, float y = 0) : IMoveble
{
    public float x = x;
    public float y = y;
    public void Move(int dx, int dy)
    {
        x += dx;
        y += dy;
    }
}
