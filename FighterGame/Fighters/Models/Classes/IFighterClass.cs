namespace Fighters.Models.Classes
{
    public interface IFighterClass : IName
    {
        int Damage { get; }
        int Health { get; }
    }
}
