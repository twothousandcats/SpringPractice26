using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters.Commands;

public class ListFightersConsoleCommand : IConsoleCommand
{
    private readonly FighterRoster _fighterRoster;

    private readonly IConsole _console;

    public ListFightersConsoleCommand( FighterRoster fighterRoster, IConsole console )
    {
        _fighterRoster = fighterRoster;
        _console = console;
    }

    public string Name => "List";

    public string Description => "Lists all fighters";

    public void Execute()
    {
        if ( _fighterRoster.Count == 0 )
        {
            _console.WriteLine( "Arena is empty!" );
            return;
        }

        for ( int i = 0; i < _fighterRoster.Count; i++ )
        {
            _console.WriteLine( $"{i + 1}. {_fighterRoster[ i ].Description}" );
        }
    }
}