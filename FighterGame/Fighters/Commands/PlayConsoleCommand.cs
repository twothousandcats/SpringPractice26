using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters.Commands;

public class PlayConsoleCommand : IConsoleCommand
{
    private readonly List<IFighter> _fighters;

    private readonly BattleRunner _battleRunner;

    private readonly IConsole _console;

    public PlayConsoleCommand( List<IFighter> fighters, BattleRunner battleRunner, IConsole console )
    {
        _fighters = fighters;
        _battleRunner = battleRunner;
        _console = console;
    }

    public string Name => "Play";

    public string Description => "Starts the battle";

    public void Execute()
    {
        if ( _fighters.Count < 2 )
        {
            _console.WriteLine( "You must add at least 2 fighters to play" );
            return;
        }

        _battleRunner.Play( _fighters );
        _fighters.Clear();
    }
}