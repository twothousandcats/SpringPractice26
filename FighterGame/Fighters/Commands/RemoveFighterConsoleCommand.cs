using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters.Commands;

public class RemoveFighterConsoleCommand : IConsoleCommand
{
    private readonly List<IFighter> _fighters;

    private readonly IConsole _console;

    public RemoveFighterConsoleCommand( List<IFighter> fighters, IConsole console )
    {
        _fighters = fighters;
        _console = console;
    }

    public string Name => "Remove";

    public string Description => "Removes a fighter";

    public void Execute()
    {
        _console.WriteLine( "Enter fighter index:" );
        string? input = _console.ReadLine(); // from 1
        if ( !int.TryParse( input, out int index ) || index < 1 || index > _fighters.Count )
        {
            _console.WriteLine( "Invalid fighter index" );
            return;
        }

        IFighter fighter = _fighters[ index - 1 ];
        _fighters.Remove( fighter );
        _console.WriteLine( $"Removed fighter: {fighter.Name}" );
    }
}