using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters.Commands;

public class RemoveFighterConsoleCommand : IConsoleCommand
{
    private readonly List<IFighter> _arena;

    private readonly IConsole _console;

    public RemoveFighterConsoleCommand( List<IFighter> arena, IConsole console )
    {
        _arena = arena;
        _console = console;
    }

    public string Name => "Remove";

    public string Description => "Removes a fighter";

    public void Execute()
    {
        _console.WriteLine( "Enter fighter index:" );
        string? input = _console.ReadLine(); // from 1
        if ( !int.TryParse( input, out int index ) || index < 1 || index > _arena.Count )
        {
            _console.WriteLine( "Invalid fighter index" );
            return;
        }

        IFighter fighter = _arena[ index - 1 ];
        _arena.Remove( fighter );
        _console.WriteLine( $"Removed fighter: {fighter.Name}" );
    }
}