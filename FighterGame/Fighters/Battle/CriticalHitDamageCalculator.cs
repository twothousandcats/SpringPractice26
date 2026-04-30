using Fighters.Models.Fighters;

namespace Fighters.Battle
{
    public class CriticalHitDamageCalculator : IDamageCalculator
    {
        private readonly IDamageCalculator _inner;
        private readonly Random _random;
        private readonly double _chance;
        private readonly double _multiplier;

        public CriticalHitDamageCalculator(
            IDamageCalculator inner,
            Random random,
            double chance = 0.15,
            double multiplier = 2.0
        )
        {
            if (chance is < 0 or > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(chance), "Chance must be in [0, 1]");
            }

            if (multiplier < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(multiplier), "Multiplier must be >= 1");
            }

            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _random = random ?? throw new ArgumentNullException(nameof(random));
            _chance = chance;
            _multiplier = multiplier;
        }

        public int Calculate(IFighter attacker, IFighter defender)
        {
            int baseDamage = _inner.Calculate(attacker, defender);
            bool isCrit = _random.NextDouble() < _chance;
            return isCrit
                ? (int)Math.Round(baseDamage * _multiplier)
                : baseDamage;
        }
    }
}
