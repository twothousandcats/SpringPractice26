using Fighters.UI;

namespace Fighters.Commands;

public class CommandLoop
{
    private readonly CommandRegistry _registry;

    private readonly IGameLoop _gameLoop;

    private readonly IConsole _console;

    public CommandLoop( CommandRegistry registry, IGameLoop gameLoop, IConsole console )
    {
        _registry = registry;
        _gameLoop = gameLoop;
        _console = console;
    }

    public void Run()
    {
        while ( _gameLoop.IsRunning )
        {
            _console.WriteLine( "" );
            _console.WriteLine( "Enter command" );
            _console.Write( "> " );
            string? input = _console.ReadLine();
            if ( string.IsNullOrEmpty( input ) )
            {
                _console.WriteLine( "Unknown command" );
                continue;
            }

            if ( !_registry.TryGet( input, out IConsoleCommand command ) )
            {
                _console.WriteLine( "Unknown command" );
                continue;
            }

            command.Execute();
        }
    }
}