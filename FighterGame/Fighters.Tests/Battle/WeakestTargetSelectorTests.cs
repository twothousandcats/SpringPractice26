using Fighters.Battle;
using Fighters.Models.Fighters;
using Fighters.Tests.TestData;

namespace Fighters.Tests.Battle;

public class WeakestTargetSelectorTests
{
    [Fact]
    public void Pick_MultipleAliveOpponents_ReturnsLowestHealth()
    {
        // Arrange
        TestFighter attacker = FighterMother.CreateDefault( "Attacker" );
        TestFighter healthy = new TestFighter
        {
            Name = "Healthy",
            CurrentHealth = 100
        };

        TestFighter wounded = new TestFighter
        {
            Name = "Wounded",
            CurrentHealth = 30
        };

        WeakestTargetSelector sut = new WeakestTargetSelector();

        // Act
        IFighter? picked = sut.Pick( attacker, new[] { attacker, healthy, wounded } );

        // Assert
        Assert.Same( wounded, picked );
    }

    [Fact]
    public void Pick_OnlySelfAndDead_ReturnsNull()
    {
        // Arrange
        TestFighter attacker = FighterMother.CreateDefault( "Attacker" );
        TestFighter dead = FighterMother.CreateDefault( "Dead" );
        WeakestTargetSelector sut = new WeakestTargetSelector();

        // Act
        IFighter? picked = sut.Pick( attacker, new[] { attacker, dead } );

        // Assert
        Assert.Null( picked );
    }

    [Fact]
    public void Pick_LowestHealthIsAttackerItself_NeverTargetsItself()
    {
        // Arrange
        TestFighter attacker = new TestFighter
        {
            Name = "Attacker",
            CurrentHealth = 1
        };

        TestFighter healthy = new TestFighter
        {
            Name = "Healthy",
            CurrentHealth = 100
        };

        WeakestTargetSelector sut = new WeakestTargetSelector();

        // Act
        IFighter? picked = sut.Pick( attacker, new[] { attacker, healthy } );

        // Assert
        Assert.Same( healthy, picked );
    }

    [Fact]
    public void Pick_OnlyAttackerPresent_ReturnsNull()
    {
        // Arrange
        TestFighter attacker = FighterMother.CreateDefault( "Attacker" );
        WeakestTargetSelector sut = new WeakestTargetSelector();

        // Act
        IFighter? picked = sut.Pick( attacker, new[] { attacker, attacker } );

        // Assert
        Assert.Null( picked );
    }
}