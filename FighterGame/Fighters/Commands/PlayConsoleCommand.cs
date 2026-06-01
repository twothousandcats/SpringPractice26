using Fighters.Battle;
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
        if ( _fighterRoster.Fighters.Count < 2 )
        {
            _console.WriteLine( "You must add at least 2 fighters to play" );
            return;
        }

        BattleResult battleResult = _battleRunner.Play( _fighterRoster.Fighters );
        _console.WriteLine( DescribeOutcome( battleResult ) );
        _fighterRoster.Clear();
    }

    private static string DescribeOutcome( BattleResult battleResult )
    {
        string winnerName = battleResult.Winner?.Name ?? "Nobody";

        return battleResult.Outcome switch
        {
            BattleOutcome.Victory => $"{winnerName} wins the battle!",
            BattleOutcome.Stalemate => $"Stalemate. {winnerName} prevails with the most HP.",
            BattleOutcome.RoundLimitReached => $"Round limit reached. {winnerName} prevails with the most HP",
            _ => throw new ArgumentOutOfRangeException( nameof( battleResult ) ),
        };
    }
}