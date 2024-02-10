namespace SpaceBattle.Api;

public class DefaultHandler : ICommand
{
    public void Execute()
    {
    }
}
public class ExceptionHandler
{
    private static readonly Dictionary<
        Type, // Command
        Dictionary<
            Type, // Exception
            Func<ICommand, Exception, ICommand>
        >
    > store = [];

    public ICommand Handle(ICommand cmd, Exception e)
    {
        Type ct = cmd.GetType();
        Type et = e.GetType();

        try
        {
            return store[ct][et](cmd, e);

        }
        catch (Exception)
        {
            return new DefaultHandler();
        }
    }

    public void RegisterHandler(Type c, Type e, Func<ICommand, Exception, ICommand> f)
    {

        var cmd = new Dictionary<Type, Func<ICommand, Exception, ICommand>>
        {
            { e, f }
        };
        store.Add(c, cmd);
        //store[c][e] = f;
    }
}
