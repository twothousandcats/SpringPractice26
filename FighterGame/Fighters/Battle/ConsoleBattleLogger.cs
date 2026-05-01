using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters.Battle
{
    public class ConsoleBattleLogger : IBattleLogger
    {
        private readonly IConsole _console;

        public ConsoleBattleLogger(IConsole console)
        {
            _console = console;
        }

        public void RoundStarted(int roundNumber)
        {
            _console.WriteLine("Round " + roundNumber);
        }

        public void AttackPerformed(IFighter attacker, IFighter target, int dealtDamage)
        {
            _console.WriteLine(
                $"{attacker.Name} deals {dealtDamage} damage, to {target.Name}."
            );
        }

        public void StalemateReached(IReadOnlyList<IFighter> fighters)
        {
            string names = string.Join(", ", fighters.Select(f => f.Name));
            _console.WriteLine($"Stalemate: nobody dealt damage this round ({names}).");
        }

        public void FighterDied(IFighter fighter)
        {
            _console.WriteLine($"{fighter.Name} dies.");
        }

        public void FighterWon(IFighter fighter)
        {
            _console.WriteLine($"{fighter.Name} wins.");
        }
    }
}
