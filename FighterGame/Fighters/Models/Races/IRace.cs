namespace Fighters.Models.Races
{
    public interface IRace : IName
    {
        int Damage { get; }
        int Health { get; }
        int Armor { get; }
        int Initiative { get; }
    }
}
