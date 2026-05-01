using Fighters.Battle;
using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Fighters.Tests.Helpers;
using NUnit.Framework;

namespace Fighters.Tests
{
    [TestFixture]
    public class RandomVarianceDamageCalculatorTests
    {
        private const int BaseDamage = 100;

        private class FixedDamageCalculator : IDamageCalculator
        {
            private readonly int _value;
            public FixedDamageCalculator(int value) => _value = value;
            public int Calculate(IFighter attacker, IFighter defender) => _value;
        }

        [Test]
        public void Calculate_NextDoubleZero_ReturnsLowerBound()
        {
            FakeRandom random = new FakeRandom(0.0);
            RandomVarianceDamageCalculator calc = new RandomVarianceDamageCalculator(
                new FixedDamageCalculator(BaseDamage),
                random
            );
            (IFighter attacker, IFighter defender) = Utils.MakePair();

            int damage = calc.Calculate(attacker, defender);

            Assert.That(damage, Is.EqualTo(80));
        }

        [Test]
        public void Calculate_NextDoubleNearOne_ReturnsUpperBound()
        {
            FakeRandom random = new FakeRandom(0.9999999);
            RandomVarianceDamageCalculator calc = new RandomVarianceDamageCalculator(
                new FixedDamageCalculator(BaseDamage),
                random
            );
            (IFighter attacker, IFighter defender) = Utils.MakePair();

            int damage = calc.Calculate(attacker, defender);

            Assert.That(damage, Is.EqualTo(110));
        }

        [Test]
        public void Calculate_NextDoubleHalf_ReturnsMiddle()
        {
            FakeRandom random = new FakeRandom(0.5);
            RandomVarianceDamageCalculator calc = new RandomVarianceDamageCalculator(
                new FixedDamageCalculator(BaseDamage),
                random
            );
            (IFighter attacker, IFighter defender) = Utils.MakePair();

            int damage = calc.Calculate(attacker, defender);

            Assert.That(damage, Is.EqualTo(95));
        }

        [Test]
        public void Calculate_ZeroBaseDamage_ReturnsZero()
        {
            FakeRandom random = new FakeRandom(0.5);
            RandomVarianceDamageCalculator calc = new RandomVarianceDamageCalculator(
                new FixedDamageCalculator(0),
                random
            );
            (IFighter attacker, IFighter defender) = Utils.MakePair();

            int damage = calc.Calculate(attacker, defender);

            Assert.That(damage, Is.EqualTo(0));
        }
    }
}
