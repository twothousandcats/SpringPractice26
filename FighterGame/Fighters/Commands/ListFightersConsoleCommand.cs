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
        IReadOnlyList<IFighter> fighters = _fighterRoster.Fighters;
        if ( fighters.Count == 0 )
        {
            _console.WriteLine( "Arena is empty!" );
            return;
        }

        for ( int i = 0; i < fighters.Count; i++ )
        {
            _console.WriteLine( $"{i + 1}. {fighters[ i ].Description}" );
        }
    }
}