using Fighters.Commands;

namespace Fighters.Tests.Helpers;

public class StubConsoleCommand : IConsoleCommand
{
    private readonly Action _action;

    public StubConsoleCommand( string name, string description, Action? action = null )
    {
        Name = name;
        Description = description;
        _action = action ?? ( () => { } );
    }

    public string Name { get; }

    public string Description { get; }

    public int ExecutedCount { get; private set; }

    public void Execute()
    {
        ExecutedCount++;
        _action();
    }
}