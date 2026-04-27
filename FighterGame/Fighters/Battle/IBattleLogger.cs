using Fighters.Models.Fighters;

namespace Fighters.Battle
{
    public interface IBattleLogger
    {
        void RoundStarted(int roundNumber);

        void AttackPerformed(
            IFighter attacker,
            IFighter target,
            int dealtDamage,
            int receivedDamage
        );

        void FighterDied(IFighter fighter);
        void FighterWon(IFighter fighter);
    }
}
