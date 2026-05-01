using Fighters.Models.Fighters;

namespace Fighters.Battle
{
    public class ConsoleBattleLogger : IBattleLogger
    {
        public void RoundStarted(int roundNumber)
        {
            Console.WriteLine("Round " + roundNumber);
        }

        public void AttackPerformed(IFighter attacker, IFighter target, int dealtDamage)
        {
            Console.WriteLine(
                $"{attacker.Name} deals {dealtDamage} damage, to {target.Name}."
            );
        }

        public void Stalemate(IReadOnlyList<IFighter> fighters)
        {
            string names = string.Join(", ", fighters.Select(f => f.Name));
            Console.WriteLine($"Stalemate: nobody dealt damage this round ({names}).");
        }

        public void FighterDied(IFighter fighter)
        {
            Console.WriteLine($"{fighter.Name} dies.");
        }

        public void FighterWon(IFighter fighter)
        {
            Console.WriteLine($"{fighter.Name} wins.");
        }
    }
}
