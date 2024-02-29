namespace SpaceBattle.Api.Commands;

public class IocRegisterCommand(IDictionary<string, object> dependencies,
                          string key,
                          Func<object[], object> action) : ICommand
{
    private readonly IDictionary<string, object> dependencies = dependencies;
    private readonly string key = key;
    private readonly Func<object[], object> action = action;

    public void Execute()
    {
        dependencies.Add(key, action);
    }
}