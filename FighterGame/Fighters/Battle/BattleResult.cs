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

    public static BattleResult Victory( IFighter winner )
    {
        return new BattleResult( BattleOutcome.Victory, winner );
    }

    public static BattleResult Stalemate( IFighter winner )
    {
        return new BattleResult( BattleOutcome.Stalemate, winner );
    }

    public static BattleResult RoundLimitReached( IFighter winner )
    {
        return new BattleResult( BattleOutcome.RoundLimitReached, winner );
    }
}