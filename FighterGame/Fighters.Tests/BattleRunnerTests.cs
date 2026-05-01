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
    public class BattleRunnerTests
    {
        [Test]
        public void Play_TwoEqualFighters_FirstFighterWins()
        {
            BattleRunner battleRunner = new BattleRunner(new SilentBattleLogger(), new WeakestTargetSelector(),
                new PlainDamageCalculator());
            IFighter fighterA = Utils.CreateFighter("fighterA");
            IFighter fighterB = Utils.CreateFighter("fighterB");

            IFighter winner = battleRunner.Play(new[] { fighterA, fighterB });

            Assert.That(winner.Name, Is.EqualTo(fighterA.Name));
        }

        [Test]
        public void Play_TwoEqualFighters_SecondFighterDies()
        {
            BattleRunner battleRunner = new BattleRunner(new SilentBattleLogger(), new WeakestTargetSelector(),
                new PlainDamageCalculator());
            IFighter fighterA = Utils.CreateFighter("fighterA");
            IFighter fighterB = Utils.CreateFighter("fighterB");

            battleRunner.Play(new[] { fighterA, fighterB });

            Assert.That(fighterA.CurrentHealth, Is.GreaterThan(0));
            Assert.That(fighterB.CurrentHealth, Is.EqualTo(0));
        }

        [Test]
        public void Play_StrongerFighterWins()
        {
            BattleRunner battleRunner = new BattleRunner(new SilentBattleLogger(), new WeakestTargetSelector(),
                new PlainDamageCalculator());
            IFighter weak = Utils.CreateFighter("Weak");
            IFighter strong = new Fighter("Strong", new Orc(), new Mercenary(), new Axe(), new PlateArmor());

            IFighter winner = battleRunner.Play(new[] { weak, strong });

            Assert.That(winner.Name, Is.EqualTo(strong.Name));
            Assert.That(weak.IsAlive, Is.False);
        }

        [Test]
        public void Play_FewerThanTwoFighters_Throws()
        {
            BattleRunner runner = new BattleRunner(
                new SilentBattleLogger(), new WeakestTargetSelector(), new PlainDamageCalculator());

            Assert.That(() => runner.Play(new[] { Utils.CreateFighter("solo") }),
                Throws.ArgumentException
            );
        }

        [Test]
        public void Play_NullList_Throws()
        {
            BattleRunner runner = new BattleRunner(
                new SilentBattleLogger(), new WeakestTargetSelector(), new PlainDamageCalculator()
            );

            Assert.That(() => runner.Play(null!), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Play_ThreeFighters_ReturnsLastSurvivor()
        {
            BattleRunner runner = new BattleRunner(
                new SilentBattleLogger(), new WeakestTargetSelector(), new PlainDamageCalculator()
            );
            IFighter weakA = Utils.CreateFighter("WeakA");
            IFighter weakB = Utils.CreateFighter("WeakB");
            IFighter strong = new Fighter("Strong", new Orc(), new Mercenary(), new Axe(), new PlateArmor());

            IFighter winner = runner.Play(new[] { weakA, weakB, strong });

            Assert.That(winner, Is.SameAs(strong));
            Assert.That(weakA.IsAlive, Is.False);
            Assert.That(weakB.IsAlive, Is.False);
        }
    }
}
