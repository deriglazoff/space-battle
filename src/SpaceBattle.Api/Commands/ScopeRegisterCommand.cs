using System.Collections.Concurrent;

namespace SpaceBattle.Api.Commands;

public class ScopeRegisterCommand(ConcurrentDictionary<string, IoC.Scope> scopesCollection, string scopeName) : ICommand
{
    private readonly ConcurrentDictionary<string, IoC.Scope> scopesCollection = scopesCollection;
    private readonly string scopeName = scopeName;

    public void Execute()
    {
        if (!scopesCollection.TryAdd(scopeName, new IoC.Scope(scopeName)))
        {
            throw new Exception($"Scope {scopeName} already registered");
        }
    }
}