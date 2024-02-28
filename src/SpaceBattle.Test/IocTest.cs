using SpaceBattle.Api.Commands;
using SpaceBattle.Api;
using Xunit;
using System.Numerics;

namespace SpaceBattle.Test;

public class IocTest {
    [Fact]
    public void ErrorThrowsWhenOperationKeyNotFound()
    {
        var ioc = new IoC();

        var act = () => ioc.Resolve<object>("test");

        Assert.Throws<Exception>(act);
    }

    [Fact]
    public void MoveCommandRegistersAndResolves()
    {
        var ioc = new IoC();
        var ship = new SpaceShip
        {
            Fuel = 10,
            NeedFuel = 10,
            Position = new Vector2(12, 5),
            Velocity = new Vector2(-7, 3)
        };
        ioc.Resolve<ICommand>("IoC.Register",
                              "Move",
                              (Func<object[], object>)(args => new MacroCommand(new CheckFuelCommand((IFuel)args[0]), new BurnFuelCommand((IFuel)args[0]), new MoveCommand((IMovable)args[0]))))
           .Execute();

        var moveCommand = ioc.Resolve<ICommand>("Move", ship);

        Assert.IsType<MacroCommand>(moveCommand);
    }

    [Fact]
    public void ScopeRegisters()
    {
        var ioc = new IoC();

        Assert.Empty(ioc.Scopes);
        ioc.Resolve<ICommand>("Scopes.New", "id_1")
           .Execute();

        Assert.NotEmpty(ioc.Scopes);
    }

    [Fact]
    public void ErrorThrowsWhenTryToRegisterDuplicatedScope()
    {
        var ioc = new IoC();

        ioc.Resolve<ICommand>("Scopes.New", "id_1")
           .Execute();
        Action act = () => ioc.Resolve<ICommand>("Scopes.New", "id_1")
                              .Execute();

        Assert.Throws<Exception>(act);
    }

    [Fact]
    public void RegisteredScopeSetsAsCurrent()
    {
        var ioc = new IoC();

        ioc.Resolve<ICommand>("Scopes.New", "id_1")
           .Execute();
        ioc.Resolve<ICommand>("Scopes.Current", "id_1")
           .Execute();

        Assert.Equal("id_1", ioc.CurrentScope.Value?.Name);
    }

    [Fact]
    public void ErrorThrowsWhenTryingSetNotRegisteredScope()
    {
        var ioc = new IoC();

        Action act = () => ioc.Resolve<ICommand>("Scopes.Current", "id_1")
                              .Execute();

        Assert.Throws<Exception>(act);
    }

    [Fact]
    public void CurrentScopeSetsForEachThread()
    {
        var ioc = new IoC();

        ioc.Resolve<ICommand>("Scopes.New", "id_1")
           .Execute();
        ioc.Resolve<ICommand>("Scopes.New", "id_2")
           .Execute();

        Task.Factory.StartNew(() =>
        {
            ioc.Resolve<ICommand>("Scopes.Current", "id_1")
               .Execute();

            Assert.Equal("id_1", ioc.CurrentScope.Value?.Name);
        });
        Task.Factory.StartNew(() =>
        {
            ioc.Resolve<ICommand>("Scopes.Current", "id_2")
               .Execute();

            Assert.Equal("id_2", ioc.CurrentScope.Value?.Name);
        });

        Assert.Equal("DefaultScope", ioc.CurrentScope.Value?.Name);
    }
}