using Fighters.Models.Fighters;

namespace Fighters.Battle
{
    public class ConsoleBattleLogger : IBattleLogger
    {
        public void RoundStarted(int roundNumber)
        {
            System.Console.WriteLine("Round " + roundNumber);
        }

        public void AttackPerformed(IFighter attacker, IFighter target, int dealtDamage, int receivedDamage)
        {
            System.Console.WriteLine(
                $"{attacker.Name} deals {dealtDamage} damage, {target.Name} received {receivedDamage} damage."
            );
        }

        public void FighterDied(IFighter fighter)
        {
            System.Console.WriteLine($"{fighter.Name} dies.");
        }

        public void FighterWon(IFighter fighter)
        {
            System.Console.WriteLine($"{fighter.Name} wins.");
        }
    }
}
