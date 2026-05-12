using Casino.Infrastructure;

namespace Casino.Menu.Commands;

public sealed class ExitCommand : IMenuCommand
{
    private readonly IInputOutput _io;

    private readonly Action _onExit;

    public ExitCommand( IInputOutput io, Action onExit )
    {
        _io = io;
        _onExit = onExit;
    }

    public void Execute()
    {
        _io.WriteLine( "Спасибо за игру!" );
        _onExit();
    }
}