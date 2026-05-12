using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters.Commands;

public class PlayConsoleCommand : IConsoleCommand
{
    private readonly FighterRoster _fighterRoster;

    private readonly BattleRunner _battleRunner;

    private readonly IConsole _console;

    public PlayConsoleCommand( FighterRoster fighterRoster, BattleRunner battleRunner, IConsole console )
    {
        _fighterRoster = fighterRoster;
        _battleRunner = battleRunner;
        _console = console;
    }

    public string Name => "Play";

    public string Description => "Starts the battle";

    public void Execute()
    {
        if ( _fighterRoster.Count < 2 )
        {
            _console.WriteLine( "You must add at least 2 fighters to play" );
            return;
        }

        _battleRunner.Play( _fighterRoster );
        _fighterRoster.Clear();
    }
}