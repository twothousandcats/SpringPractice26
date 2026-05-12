namespace Fighters.Models.Classes;

public interface IFighterClass : INamed
{
    int Damage { get; }

    int Health { get; }
}