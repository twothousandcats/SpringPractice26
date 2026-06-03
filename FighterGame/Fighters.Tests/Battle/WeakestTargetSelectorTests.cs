using Fighters.Battle;
using Fighters.Models.Fighters;
using Fighters.Tests.TestData;

namespace Fighters.Tests.Battle;

public class WeakestTargetSelectorTests
{
    private readonly WeakestTargetSelector _selector = new WeakestTargetSelector();

    [Fact]
    public void Pick_MultipleOpponents_ReturnsLowestHealthAlive()
    {
        IFighter attacker = FighterBuilder.CreateMock( "A" ).Object;
        IFighter healthy = FighterBuilder.CreateMock( "B", currentHealth: 100 ).Object;
        IFighter wounded = FighterBuilder.CreateMock( "C", currentHealth: 30 ).Object;

        IFighter? picked = _selector.Pick( attacker, new[] { attacker, healthy, wounded } );

        Assert.Same( wounded, picked );
    }

    [Fact]
    public void Pick_OnlySelfAndDead_ReturnsNull()
    {
        IFighter attacker = FighterBuilder.CreateMock( "A" ).Object;
        IFighter dead = FighterBuilder.CreateMock( "B", isAlive: false ).Object;

        Assert.Null( _selector.Pick( attacker, new[] { attacker, dead } ) );
    }

    [Fact]
    public void Pick_LowestHealthIsAttackerItself_NeverTargetsItself()
    {
        IFighter attacker = FighterBuilder.CreateMock( "A", currentHealth: 1 ).Object;
        IFighter other = FighterBuilder.CreateMock( "B", currentHealth: 100 ).Object;

        Assert.Same( other, _selector.Pick( attacker, new[] { attacker, other } ) );
    }

    [Fact]
    public void Pick_OnlyAttackerPresent_ReturnsNull()
    {
        IFighter attacker = FighterBuilder.CreateMock( "A" ).Object;

        Assert.Null( _selector.Pick( attacker, new[] { attacker } ) );
    }

    [Fact]
    public void Pick_AllOpponentsDead_ReturnsNull()
    {
        IFighter attacker = FighterBuilder.CreateMock( "A" ).Object;
        IFighter deadB = FighterBuilder.CreateMock( "B", isAlive: false ).Object;
        IFighter deadC = FighterBuilder.CreateMock( "C", isAlive: false ).Object;

        Assert.Null( _selector.Pick( attacker, new[] { attacker, deadB, deadC } ) );
    }
}