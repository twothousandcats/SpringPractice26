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
            IFighter fighter = new Fighter(
                "Hero",
                new Human(),
                new Knight(),
                new Fists(),
                new NoArmor()
            );

            int maxHealth = fighter.MaxHealth;

            Assert.That(maxHealth, Is.EqualTo(150));
        }

        [Test]
        public void Damage_IsRacePlusClassPlusWeapon()
        {
            IFighter fighter = new Fighter(
                "Hero",
                new Human(),
                new Knight(),
                new Sword(),
                new NoArmor()
            );

            int damageTaken = fighter.Damage;

            Assert.That(damageTaken, Is.EqualTo(1 + 5 + 15));
        }

        [Test]
        public void Armor_IsRacePlusArmor()
        {
            IFighter fighter = new Fighter(
                "Dude",
                new Dwarf(),
                new Knight(),
                new Fists(),
                new PlateArmor()
            );

            int armor = fighter.Armor;

            Assert.That(armor, Is.EqualTo(3 + 15));
        }

        [Test]
        public void TakeDamage_ReducesHealth_NeverBelowZero()
        {
            IFighter fighter = new Fighter(
                "Dude",
                new Dwarf(),
                new Knight(),
                new Fists(),
                new PlateArmor()
            );

            fighter.TakeDamage(99999);
            int currentHealth = fighter.CurrentHealth;
            bool isAlive = fighter.IsAlive;

            Assert.That(currentHealth, Is.EqualTo(0));
            Assert.That(isAlive, Is.False);
        }

        [Test]
        public void Constructor_ThrowsOnEmptyName()
        {
            Assert.That(
                () => new Fighter("  ", new Human(), new Knight(), new Fists(), new NoArmor()),
                Throws.ArgumentException
            );
        }

        [Test]
        public void Constructor_StartsAtFullHealth()
        {
            IFighter fighter = new Fighter("Hero", new Human(), new Knight(), new Fists(), new NoArmor());

            int curHp = fighter.CurrentHealth;
            bool isFighterAlive = fighter.IsAlive;

            Assert.That(isFighterAlive, Is.True);
            Assert.That(curHp, Is.EqualTo(fighter.MaxHealth));
        }

        [Test]
        public void TakeDamage_NegativeAmount_Throws()
        {
            IFighter fighter = new Fighter("Hero", new Human(), new Knight(), new Fists(), new NoArmor());

            Assert.That(() => fighter.TakeDamage(-1), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void TakeDamage_PartialDamage_ReducesHealthByExactAmount()
        {
            const int FixedDamage = 30;
            IFighter fighter = new Fighter("Hero", new Human(), new Knight(), new Fists(), new NoArmor());
            int before = fighter.CurrentHealth;

            fighter.TakeDamage(FixedDamage);

            Assert.That(fighter.CurrentHealth, Is.EqualTo(before - FixedDamage));
        }

        [Test]
        public void Description_ContainsAllPartsAndHealth()
        {
            IFighter fighter = new Fighter("Hero", new Human(), new Knight(), new Sword(), new PlateArmor());

            string description = fighter.Description;

            Assert.That(description, Does.Contain("Hero"));
            Assert.That(description, Does.Contain("Human"));
            Assert.That(description, Does.Contain("Knight"));
            Assert.That(description, Does.Contain("Sword"));
            Assert.That(description, Does.Contain("Plate Armor"));
            Assert.That(description, Does.Contain("150 / 150"));
        }
    }
}
