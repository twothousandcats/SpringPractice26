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
            BattleRunner battleRunner = new BattleRunner(new SilentBattleLogger(), new WeakestTargetSelector(),
                new PlainDamageCalculator());
            IFighter fighterA = CreateFighter("fighterA");
            IFighter fighterB = CreateFighter("fighterB");

            IFighter winner = battleRunner.Play(new[] { fighterA, fighterB });

            Assert.That(winner.Name, Is.EqualTo(fighterA.Name));
        }

        [Test]
        public void Play_TwoEqualFighters_SecondFighterDies()
        {
            BattleRunner battleRunner = new BattleRunner(new SilentBattleLogger(), new WeakestTargetSelector(),
                new PlainDamageCalculator());
            IFighter fighterA = CreateFighter("fighterA");
            IFighter fighterB = CreateFighter("fighterB");

            battleRunner.Play(new[] { fighterA, fighterB });

            Assert.That(fighterA.CurrentHealth, Is.GreaterThan(0));
            Assert.That(fighterB.CurrentHealth, Is.EqualTo(0));
        }

        [Test]
        public void Play_StrongerFighterWins()
        {
            BattleRunner battleRunner = new BattleRunner(new SilentBattleLogger(), new WeakestTargetSelector(),
                new PlainDamageCalculator());
            IFighter weak = new Fighter("Weak", new Human(), new Knight(), new Fists(), new NoArmor());
            IFighter strong = new Fighter("Strong", new Orc(), new Mercenary(), new Axe(), new PlateArmor());

            IFighter winner = battleRunner.Play(new[] { weak, strong });

            Assert.That(winner.Name, Is.EqualTo(strong.Name));
            Assert.That(weak.IsAlive, Is.False);
        }
    }
}
