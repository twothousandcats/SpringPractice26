using Fighters.Battle;
using Fighters.Models.Fighters;

namespace Fighters.Tests.Helpers;

public class TestBattleLogger : IBattleLogger
{
    public void RoundStarted( int roundNumber ) { }
    public void AttackPerformed( IFighter attacker, IFighter target, int dealtDamage ) { }
    public void StalemateReached( IReadOnlyList<IFighter> fighters ) { }
    public void FighterDied( IFighter fighter ) { }
    public void FighterWon( IFighter fighter ) { }
}