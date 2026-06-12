namespace Fighters.Tests.TestData;

public class FighterMother
{
    public static TestFighter CreateDefault( string name = "Fighter" ) => new TestFighter
    {
        Name = name
    };

    public static TestFighter CreateWounded(
        string name,
        int currentHealth
    ) => new TestFighter
    {
        Name = name,
        CurrentHealth = currentHealth
    };

    public static TestFighter CreateDead( string name ) => new TestFighter
    {
        Name = name,
        CurrentHealth = 0
    };
}