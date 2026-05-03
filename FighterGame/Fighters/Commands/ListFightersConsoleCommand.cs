using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters.Commands;

public class ListFightersConsoleCommand : IConsoleCommand
{
    private readonly List<IFighter> _fighters;

    private readonly IConsole _console;

    public ListFightersConsoleCommand( List<IFighter> fighters, IConsole console )
    {
        _fighters = fighters;
        _console = console;
    }

    public string Name => "List";

    public string Description => "Lists all fighters";

    public void Execute()
    {
        if ( _fighters.Count == 0 )
        {
            _console.WriteLine( "Arena is empty!" );
            return;
        }

        for ( int i = 0; i < _fighters.Count; i++ )
        {
            _console.WriteLine( $"{i + 1}. {_fighters[ i ].Description}" );
        }
    }
}