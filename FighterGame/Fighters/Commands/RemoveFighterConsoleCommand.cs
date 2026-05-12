using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters.Commands;

public class RemoveFighterConsoleCommand : IConsoleCommand
{
    private readonly FighterRoster _fighterRoster;

    private readonly IConsole _console;

    public RemoveFighterConsoleCommand( FighterRoster fighterRoster, IConsole console )
    {
        _fighterRoster = fighterRoster;
        _console = console;
    }

    public string Name => "Remove";

    public string Description => "Removes a fighter";

    public void Execute()
    {
        _console.WriteLine( "Enter fighter index:" );
        string? input = _console.ReadLine(); // from 1
        if ( !int.TryParse( input, out int index ) || index < 1 || index > _fighterRoster.Count )
        {
            _console.WriteLine( "Invalid fighter index" );
            return;
        }

        IFighter fighter = _fighterRoster[ index - 1 ];
        _fighterRoster.RemoveAt( index - 1 );
        _console.WriteLine( $"Removed fighter: {fighter.Name}" );
    }
}