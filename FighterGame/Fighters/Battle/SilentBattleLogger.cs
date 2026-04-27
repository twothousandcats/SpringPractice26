using Fighters.Models.Fighters;

namespace Fighters.Battle
{
    public class SilentBattleLogger : IBattleLogger
    {
        public void RoundStarted(int roundNumber) { }

        public void AttackPerformed(IFighter attacker, IFighter target, int dealtDamage, int receivedDamage) { }

        public void FighterDied(IFighter fighter) { }
        public void FighterWon(IFighter fighter) { }
    }
}
