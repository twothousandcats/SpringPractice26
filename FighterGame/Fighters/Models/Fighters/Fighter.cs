using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.Models.Fighters
{
    public class Fighter : IFighter
    {
        private readonly IRace _race;
        private readonly IFighterClass _class;
        private readonly IWeapon _weapon;
        private readonly IArmor _armor;

        private int _currentHealth;

        public Fighter(
            string name,
            IRace race,
            IFighterClass fighterClass,
            IWeapon weapon,
            IArmor armor)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Fighter must not be empty", nameof(name));
            }

            Name = name;
            _race = race ?? throw new ArgumentNullException(nameof(race));
            _class = fighterClass ?? throw new ArgumentNullException(nameof(fighterClass));
            _weapon = weapon ?? throw new ArgumentNullException(nameof(weapon));
            _armor = armor ?? throw new ArgumentNullException(nameof(armor));

            _currentHealth = GetMaxHealth();
        }

        public string Name { get; }

        public int GetCurrentHealth()
        {
            return _currentHealth;
        }

        public int GetMaxHealth()
        {
            return _race.Health + _class.Health;
        }

        public int CalculateDamage()
        {
            return _race.Damage + _class.Damage + _weapon.Damage;
        }

        public int CalculateArmor()
        {
            return _race.Armor + _armor.Armor;
        }

        public bool IsAlive()
        {
            return _currentHealth > 0;
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(damage), "Damage cannot be negative");
            }

            int newHealth = _currentHealth - damage;
            _currentHealth = newHealth < 0 ? 0 : _currentHealth;
        }
    }
}
