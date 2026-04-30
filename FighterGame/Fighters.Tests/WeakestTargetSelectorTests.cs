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
    public class WeakestTargetSelectorTests
    {
        private static Fighter Make(string name)
        {
            return new Fighter(name, new Human(), new Knight(), new Fists(), new NoArmor());
        }

        [Test]
        public void Pick_ReturnsOpponentWithLowestHealth()
        {
            WeakestTargetSelector selector = new WeakestTargetSelector();
            Fighter attacker = Make("A");
            Fighter healthy = Make("B");
            Fighter wounded = Make("C");
            wounded.TakeDamage(50);

            IFighter? picked = selector.Pick(attacker, new[] { attacker, healthy, wounded });

            Assert.That(picked, Is.SameAs(wounded));
        }

        [Test]
        public void Pick_SkipsDeadAndSelf()
        {
            WeakestTargetSelector selector = new WeakestTargetSelector();
            Fighter attacker = Make("A");
            Fighter dead = Make("B");
            dead.TakeDamage(99999);

            IFighter? picked = selector.Pick(attacker, new[] { attacker, dead });

            Assert.That(picked, Is.Null);
        }
    }
}
