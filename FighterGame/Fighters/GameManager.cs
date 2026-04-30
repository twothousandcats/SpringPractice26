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

        public IFighter Play(IReadOnlyList<IFighter> fighters)
        {
            ArgumentNullException.ThrowIfNull(fighters);
            if (fighters.Count < 2)
            {
                throw new ArgumentException("At least two fighters are required", nameof(fighters));
            }

            var arena = new List<IFighter>(fighters);

            for (int round = 1; round <= MaxRounds; round++)
            {
                _logger.RoundStarted(round);

                IEnumerable<IFighter> turnOrder = arena
                    .OrderByDescending(f => f.Initiative)
                    .ToArray();

                foreach (IFighter attacker in turnOrder)
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
                    _logger.AttackPerformed(attacker, target, dealt);

                    if (!target.IsAlive)
                    {
                        _logger.FighterDied(target);
                    }
                }
            }

            throw new InvalidOperationException("Battle did not finish within the round limit!");
        }

        private static IFighter? PickWeakestOpponent(IFighter self, IReadOnlyList<IFighter> arena)
        {
            IFighter? weakest = null;
            foreach (IFighter candidate in arena)
            {
                if (!candidate.IsAlive || ReferenceEquals(candidate, self))
                {
                    continue;
                }

                if (weakest is null || candidate.CurrentHealth < weakest.CurrentHealth)
                {
                    weakest = candidate;
                }
            }

            return weakest;
        }

        private static int ApplyAttack(IFighter attacker, IFighter defender)
        {
            int damage = Math.Max(attacker.Damage - defender.Armor, 0);
            defender.TakeDamage(damage);
            return damage;
        }
    }
}
