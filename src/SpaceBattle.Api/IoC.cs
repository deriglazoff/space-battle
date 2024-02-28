using SpaceBattle.Api.Commands;
using System.Collections.Concurrent;

namespace SpaceBattle.Api;

public class IoC : IResolvable
{
    public ConcurrentDictionary<string, Scope> Scopes { get; }
    public ThreadLocal<Scope> CurrentScope { get; }

    public IoC()
    {
        Scopes = new();
        CurrentScope = new() { Value = new Scope("DefaultScope") };
        new IocRegisterCommand(CurrentScope.Value?.Dependencies,
                                        "IoC.Register",
                                        args => new IocRegisterCommand(CurrentScope.Value?.Dependencies,
                                          args[0] as string,
                                          args[1] as Func<object[], object>)
                             ).Execute();

        new IocRegisterCommand(CurrentScope.Value?.Dependencies,
                                "Scopes.New",
                                args => new ScopeRegisterCommand(Scopes, args[0].ToString())
                     ).Execute();

        new IocRegisterCommand(CurrentScope.Value?.Dependencies,
                        "Scopes.Current",
                        args => new ScopeCurrentSetCommand(this, Scopes, args[0].ToString())
             ).Execute();
    }
    public T? Resolve<T>(string key, params object[] args)
    {
        if (CurrentScope.Value == null || key == null || !CurrentScope.Value.Dependencies.TryGetValue(key, out object? value))
        {
            throw new Exception($"No operation with key {key}");
        }

        var function = (Func<object[], object>)value;
        return (T)function.Invoke(args);
    }

    public class Scope(string name)
    {
        public string Name { get; } = name;
        public IDictionary<string, object> Dependencies { get; } = new Dictionary<string, object>();
    }
}