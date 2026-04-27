using Fighters.Models.Weapons;

namespace Fighters.Models.Weapons
{
    public class Sword : IWeapon
    {
        public string Name => "Sword";
        public int Damage => 15;
    }
}
