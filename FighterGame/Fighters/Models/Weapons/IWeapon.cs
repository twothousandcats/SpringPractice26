namespace Fighters.Models.Weapons
{
    public interface IWeapon : IName
    {
        int Damage { get; }
    }
}
