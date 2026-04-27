using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using NUnit.Framework;

namespace Fighters.Tests
{
    [TestFixture]
    public class FighterTests
    {
        // AAA
        [Test]
        public void MaxHealth_IsRacePlusClassHealth()
        {
            Fighter fighter = new Fighter(
                "Hero",
                new Human(),
                new Knight(),
                new Fists(),
                new NoArmor()
            );

            int maxHealth = fighter.GetMaxHealth();

            Assert.That(maxHealth, Is.EqualTo(150));
        }

        [Test]
        public void Damage_IsRacePlusClassPlusWeapon()
        {
            Fighter fighter = new Fighter(
                "Hero",
                new Human(),
                new Knight(),
                new Sword(),
                new NoArmor()
            );

            int damageTaken = fighter.CalculateDamage();

            Assert.That(damageTaken, Is.EqualTo(1 + 5 + 15));
        }

        [Test]
        public void Armor_IsRacePlusArmor()
        {
            Fighter fighter = new Fighter(
                "Dude",
                new Dwarf(),
                new Knight(),
                new Fists(),
                new PlateArmor()
            );

            int armor = fighter.CalculateArmor();

            Assert.That(armor, Is.EqualTo(3 + 15));
        }

        [Test]
        public void TakeDamage_ReducesHealth_NeverBelowZero()
        {
            Fighter fighter = new Fighter(
                "Dude",
                new Dwarf(),
                new Knight(),
                new Fists(),
                new PlateArmor()
            );

            fighter.TakeDamage(99999);
            int currentHealth = fighter.GetCurrentHealth();
            bool isAlive = fighter.IsAlive();

            Assert.That(currentHealth, Is.EqualTo(0));
            Assert.That(isAlive, Is.False);
        }
    }
}
