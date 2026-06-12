using Fighters.Models.Fighters;

namespace Fighters.Tests.TestData;

public sealed class TestFighter : IFighter
{
    public string Name { get; init; } = "Fighter";

    public string Description { get; init; } = "Fighter";

    public int MaxHealth { get; init; } = 100;

    public int CurrentHealth { get; set; } = 100;

    public int Damage { get; init; } = 10;

    public int Armor { get; init; } = 0;

    public int Initiative { get; init; } = 0;

    public bool IsAlive => CurrentHealth > 0;

    public void TakeDamage( int damage )
    {
        if ( damage < 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( damage ) );
        }

        CurrentHealth = Math.Max( 0, CurrentHealth - damage );
    }
}