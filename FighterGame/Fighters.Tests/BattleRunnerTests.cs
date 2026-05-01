using Fighters.Battle;
using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using NUnit.Framework;

namespace Fighters.Tests
{
    [TestFixture]
    public class BattleRunnerTests
    {
        private static Fighter CreateFighter(string Name)
        {
            return new Fighter(Name, new Human(), new Knight(), new Fists(), new NoArmor());
        }

        [Test]
        public void Play_TwoEqualFighters_FirstFighterWins()
        {
            var gameManager = new BattleRunner(new SilentBattleLogger(), new WeakestTargetSelector(),
                new PlainDamageCalculator());
            var fighterA = CreateFighter("fighterA");
            var fighterB = CreateFighter("fighterB");

            var winner = gameManager.Play(new[] { fighterA, fighterB });

            Assert.That(winner.Name, Is.EqualTo(fighterA.Name));
        }

        [Test]
        public void Play_TwoEqualFighters_SecondFighterDies()
        {
            var gameManager = new BattleRunner(new SilentBattleLogger(), new WeakestTargetSelector(),
                new PlainDamageCalculator());
            var fighterA = CreateFighter("fighterA");
            var fighterB = CreateFighter("fighterB");

            gameManager.Play(new[] { fighterA, fighterB });

            Assert.That(fighterA.CurrentHealth, Is.GreaterThan(0));
            Assert.That(fighterB.CurrentHealth, Is.EqualTo(0));
        }

        [Test]
        public void Play_StrongerFighterWins()
        {
            var gameManager = new BattleRunner(new SilentBattleLogger(), new WeakestTargetSelector(),
                new PlainDamageCalculator());
            var weak = new Fighter("Weak", new Human(), new Knight(), new Fists(), new NoArmor());
            var strong = new Fighter("Strong", new Orc(), new Mercenary(), new Axe(), new PlateArmor());

            var winner = gameManager.Play(new[] { weak, strong });

            Assert.That(winner.Name, Is.EqualTo(strong.Name));
            Assert.That(weak.IsAlive, Is.False);
        }
    }
}
