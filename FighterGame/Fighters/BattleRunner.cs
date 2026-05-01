using Fighters.Battle;
using Fighters.Models.Fighters;

namespace Fighters
{
    public class BattleRunner
    {
        private const int MaxRounds = 1000;

        private readonly IBattleLogger _logger;
        private readonly ITargetSelector _targetSelector;
        private readonly IDamageCalculator _damageCalculator;

        public BattleRunner(IBattleLogger logger, ITargetSelector targetSelector, IDamageCalculator damageCalculator)
        {
            _logger = logger;
            _targetSelector = targetSelector;
            _damageCalculator = damageCalculator;
        }

        public IFighter Play(IReadOnlyList<IFighter> fighters)
        {
            ArgumentNullException.ThrowIfNull(fighters);
            if (fighters.Count < 2)
            {
                throw new ArgumentException("At least two fighters are required", nameof(fighters));
            }

            List<IFighter> arena = new List<IFighter>(fighters);

            for (int round = 1; round <= MaxRounds; round++)
            {
                _logger.RoundStarted(round);
                int totalDamageThisRound = 0;

                IEnumerable<IFighter> turnOrder = arena
                    .OrderByDescending(f => f.Initiative)
                    .ToArray();

                foreach (IFighter attacker in turnOrder)
                {
                    if (!attacker.IsAlive)
                    {
                        continue;
                    }

                    IFighter? target = _targetSelector.Pick(attacker, arena);
                    if (target is null)
                    {
                        _logger.FighterWon(attacker);
                        return attacker;
                    }

                    int dealt = ApplyAttack(attacker, target);
                    totalDamageThisRound += dealt;
                    _logger.AttackPerformed(attacker, target, dealt);

                    if (!target.IsAlive)
                    {
                        _logger.FighterDied(target);
                    }
                }

                if (totalDamageThisRound == 0)
                {
                    IFighter[] survivors = arena.Where(f => f.IsAlive).ToArray();
                    _logger.StalemateReached(survivors);
                    throw new InvalidOperationException("Battle ended in stalemate.");
                }
            }

            throw new InvalidOperationException("Battle did not finish within the round limit!");
        }

        private int ApplyAttack(IFighter attacker, IFighter defender)
        {
            int damage = _damageCalculator.Calculate(attacker, defender);
            defender.TakeDamage(damage);
            return damage;
        }
    }
}
