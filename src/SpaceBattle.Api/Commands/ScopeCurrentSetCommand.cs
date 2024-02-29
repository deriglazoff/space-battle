using System.Collections.Concurrent;

namespace SpaceBattle.Api.Commands;

public class ScopeCurrentSetCommand(IoC ioC, ConcurrentDictionary<string, IoC.Scope> scopes, string scopeIdToSet) : ICommand
{
    private readonly IoC ioC = ioC;
    private readonly ConcurrentDictionary<string, IoC.Scope> scopes = scopes;
    private readonly string scopeIdToSet = scopeIdToSet;

    public void Execute()
    {
        if (!scopes.TryGetValue(scopeIdToSet, out var scopeToSet))
        {
            throw new Exception($"Scope {scopeIdToSet} not registered");
        }

        ioC.CurrentScope.Value = scopeToSet;
    }
}