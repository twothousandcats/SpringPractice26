namespace Fighters.Models.Fighters;

public interface IFighter : IName
{
    int CurrentHealth { get; }

    int MaxHealth { get; }

    int Damage { get; }

    int Armor { get; }

    bool IsAlive { get; }

    int Initiative { get; }

    string Description { get; }

    void TakeDamage( int damage );
}