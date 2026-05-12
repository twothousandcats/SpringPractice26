using Fighters.Models.Fighters;

namespace Fighters.Battle;

public interface IBattleLogger
{
    void LogAnnounceRound( int roundNumber );
    void LogPerformAttack(
        IFighter attacker,
        IFighter target,
        int dealtDamage
    );
    void LogReachStalemate( IReadOnlyList<IFighter> fighters );
    void LogFighterDied( IFighter fighter );
    void LogFighterWon( IFighter fighter );
}