using Fighters.Models.Fighters;

namespace Fighters.Battle;

public sealed class BattleResult
{
    private BattleResult( BattleOutcome battleOutcome, IFighter? fighter )
    {
        Outcome = battleOutcome;
        Winner = fighter;
    }

    public BattleOutcome Outcome { get; }

    public IFighter? Winner { get; }

    public static BattleResult Victory( IFighter fighter )
    {
        return new BattleResult( BattleOutcome.Victory, fighter );
    }

    public static BattleResult Stalemate() => new BattleResult( BattleOutcome.Stalemate, null );

    public static BattleResult RoundLimitReached() => new BattleResult( BattleOutcome.RoundLimitReached, null );
}