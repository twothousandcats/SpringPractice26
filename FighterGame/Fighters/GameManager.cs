using Fighters.Battle;
using Fighters.Models.Fighters;

namespace Fighters
{
    public class GameManager
    {
        private const int MaxRounds = 1000;

        private readonly IBattleLogger _logger;

        public GameManager() : this(new ConsoleBattleLogger()) { }

        public GameManager(IBattleLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IFighter Play(IFighter first, IFighter second)
        {
            ArgumentNullException.ThrowIfNull(first);
            ArgumentNullException.ThrowIfNull(second);

            for (int round = 1; round <= MaxRounds; round++)
            {
                _logger.RoundStarted(round);
                ExchangeBlows(first, second);

                IFighter? winner = GetWinner(first, second);
                if (winner != null)
                {
                    return winner;
                }
            }

            throw new InvalidOperationException("Battle did not finish within the round limit!");
        }

        public IFighter Play(IReadOnlyList<IFighter> fighters)
        {
            ArgumentNullException.ThrowIfNull(fighters);

            List<IFighter> arena = new List<IFighter>(fighters);

            for (int round = 1; round <= MaxRounds; round++)
            {
                _logger.RoundStarted(round);
                IEnumerable<IFighter> fightersTurnOrder = arena
                    .OrderByDescending(f => f.Initiative)
                    .ToArray();

                foreach (IFighter attacker in fightersTurnOrder)
                {
                    if (!attacker.IsAlive)
                    {
                        continue;
                    }

                    IFighter? target = PickWeakestOpponent(attacker, arena);
                    if (target is null)
                    {
                        _logger.FighterWon(attacker);
                        return attacker;
                    }

                    int dealt = ApplyAttack(attacker, target);
                    int received = target.IsAlive
                        ? ApplyAttack(target, attacker)
                        : 0;
                    _logger.AttackPerformed(attacker, target, dealt, received);

                    if (!target.IsAlive)
                    {
                        _logger.FighterWon(target);
                    }
                }
            }

            throw new InvalidOperationException("Battle did not finish within the round limit!");
        }

        private static IFighter? PickWeakestOpponent(IFighter self, IReadOnlyList<IFighter> fighters)
        {
            IFighter? weakestOpponent = null;
            foreach (IFighter fighter in fighters)
            {
                if (!fighter.IsAlive || ReferenceEquals(fighter, self))
                {
                    continue;
                }

                if (weakestOpponent is null || fighter.CurrentHealth < weakestOpponent.CurrentHealth)
                {
                    weakestOpponent = fighter;
                }
            }

            return weakestOpponent;
        }

        private IFighter? GetWinner(IFighter first, IFighter second)
        {
            if (!second.IsAlive)
            {
                Result(first, second);
                return first;
            }

            if (!first.IsAlive)
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

        private void ExchangeBlows(IFighter first, IFighter second)
        {
            IFighter attacker = first.Initiative >= second.Initiative
                ? first
                : second;
            IFighter defender = ReferenceEquals(attacker, first)
                ? second
                : first;

            int dealt = ApplyAttack(attacker, defender);
            int received = defender.IsAlive
                ? ApplyAttack(defender, attacker)
                : 0;

            _logger.AttackPerformed(attacker, defender, dealt, received);
        }

        private static int ApplyAttack(IFighter attacker, IFighter defender)
        {
            int damage = Math.Max(attacker.Damage - defender.Damage, 0);
            defender.TakeDamage(damage);
            return damage;
        }
    }
}
