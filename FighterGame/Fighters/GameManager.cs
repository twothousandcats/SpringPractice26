using Fighters.Battle;
using Fighters.Models.Fighters;

namespace Fighters
{
    public class GameManager
    {
        private const int MaxRounds = 1000;

        private readonly IBattleLogger _logger;

        public GameManager() : this(new ConsoleBattleLogger()) {}
        public GameManager(IBattleLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IFighter Play(IFighter first, IFighter second)
        {
            ArgumentNullException.ThrowIfNull(first);
            ArgumentNullException.ThrowIfNull(second);

            for (var round = 1; round <= MaxRounds; round++)
            {
                _logger.RoundStarted(round);
                ExchangeBlows(first, second);

                var winner = GetWinner(first, second);
                if (winner != null)
                {
                    return winner;
                }
            }

            throw new InvalidOperationException("Battle did not finish within the round limit!");
        }

        private IFighter? GetWinner(IFighter first, IFighter second)
        {
            if (!second.IsAlive())
            {
                Result(first, second);
                return first;
            }

            if (!first.IsAlive())
            {
                Result(second, first);
                return second;
            }

            return null;
        }

        private void Result(IFighter winner, IFighter loser)
        {
            _logger.FighterDied(loser);
            _logger.FighterWon(winner);
        }

        private void ExchangeBlows(IFighter attacker, IFighter defender)
        {
            var dealt = ApplyAttack(attacker, defender);
            var received = defender.IsAlive()
                ? ApplyAttack(defender, attacker)
                : 0;

            _logger.AttackPerformed(attacker, defender, dealt, received);
        }

        private static int ApplyAttack(IFighter attacker, IFighter defender)
        {
            var damage = Math.Max(attacker.CalculateDamage() - defender.CalculateArmor(), 0);
            defender.TakeDamage(damage);
            return damage;
        }
    }
}
