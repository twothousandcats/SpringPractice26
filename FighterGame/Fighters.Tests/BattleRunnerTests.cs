using Fighters.Battle;
using Fighters.Models.Fighters;
using Fighters.Tests.TestData;
using Moq;

namespace Fighters.Tests;

public class BattleRunnerTests
{
    private readonly Mock<IBattleLogger> _logger = new();

    private readonly Mock<ITargetSelector> _selector = new();

    private readonly Mock<IDamageCalculator> _damage = new();

    private BattleRunner CreateRunner() => new( _logger.Object, _selector.Object, _damage.Object );

    private void SelectorAlwaysReturns( IFighter? target ) => _selector
        .Setup( selector => selector.Pick( It.IsAny<IFighter>(), It.IsAny<IReadOnlyList<IFighter>>() ) )
        .Returns( target );

    [Fact]
    public void Play_NullFighters_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>( () => CreateRunner().Play( null! ) );

    [Fact]
    public void Play_FewerThanTwoFighters_ThrowsArgumentException()
    {
        IFighter solo = FighterBuilder.CreateMock().Object;

        Assert.Throws<ArgumentException>( () => CreateRunner().Play( new[] { solo } ) );
    }

    [Fact]
    public void Play_NoTargetForAttacker_ReturnsVictoryForActingFighter()
    {
        Mock<IFighter> first = FighterBuilder.CreateMock( "First", initiative: 10 );
        Mock<IFighter> second = FighterBuilder.CreateMock( "Second", initiative: 1 );
        SelectorAlwaysReturns( null );

        BattleResult result = CreateRunner().Play( new[] { first.Object, second.Object } );

        Assert.Equal( BattleOutcome.Victory, result.Outcome );
        Assert.Same( first.Object, result.Winner );
        _logger.Verify( l => l.LogAnnounceRound( 1 ), Times.Once );
        _logger.Verify( l => l.LogFighterWon( first.Object ), Times.AtLeastOnce );
    }

    [Fact]
    public void Play_HigherInitiative_ActsFirst()
    {
        Mock<IFighter> slow = FighterBuilder.CreateMock( "Slow", initiative: 1 );
        Mock<IFighter> fast = FighterBuilder.CreateMock( "Fast", initiative: 10 );
        SelectorAlwaysReturns( null );

        BattleResult result = CreateRunner().Play( new[] { slow.Object, fast.Object } );

        Assert.Same( fast.Object, result.Winner );
    }

    [Fact]
    public void Play_AttackerHitsTarget_AppliesDamageAndLogsAttack()
    {
        Mock<IFighter> attacker = FighterBuilder.CreateMock( "Attacker", initiative: 10 );
        Mock<IFighter> defender = FighterBuilder.CreateMock( "Defender", initiative: 1 );
        _damage.Setup( d => d.Calculate( attacker.Object, defender.Object ) ).Returns( 30 );
        _selector
            .SetupSequence( s => s.Pick( It.IsAny<IFighter>(), It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( defender.Object )
            .Returns( ( IFighter? )null );

        CreateRunner().Play( new[] { attacker.Object, defender.Object } );

        defender.Verify( f => f.TakeDamage( 30 ), Times.Once );
        _logger.Verify( l => l.LogPerformAttack( attacker.Object, defender.Object, 30 ), Times.Once );
    }

    [Fact]
    public void Play_TargetDies_LogsFighterDied()
    {
        Mock<IFighter> attacker = FighterBuilder.CreateMock( "Attacker", initiative: 10 );
        Mock<IFighter> defender = FighterBuilder.CreateMock( "Defender", initiative: 1, isAlive: false );
        _damage.Setup( d => d.Calculate( It.IsAny<IFighter>(), It.IsAny<IFighter>() ) ).Returns( 100 );
        _selector
            .SetupSequence( s => s.Pick( attacker.Object, It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( defender.Object )
            .Returns( ( IFighter? )null );

        BattleResult result = CreateRunner().Play( new[] { attacker.Object, defender.Object } );

        _logger.Verify( l => l.LogFighterDied( defender.Object ), Times.Once );
        Assert.Equal( BattleOutcome.Victory, result.Outcome );
        Assert.Same( attacker.Object, result.Winner );
    }

    [Fact]
    public void Play_NobodyDealsDamage_ReturnsStalemateWithStrongestSurvivor()
    {
        Mock<IFighter> a = FighterBuilder.CreateMock( "A", currentHealth: 50, initiative: 10 );
        Mock<IFighter> b = FighterBuilder.CreateMock( "B", currentHealth: 30, initiative: 1 );
        _damage.Setup( d => d.Calculate( It.IsAny<IFighter>(), It.IsAny<IFighter>() ) ).Returns( 0 );
        _selector.Setup( s => s.Pick( a.Object, It.IsAny<IReadOnlyList<IFighter>>() ) ).Returns( b.Object );
        _selector.Setup( s => s.Pick( b.Object, It.IsAny<IReadOnlyList<IFighter>>() ) ).Returns( a.Object );

        BattleResult result = CreateRunner().Play( new[] { a.Object, b.Object } );

        Assert.Equal( BattleOutcome.Stalemate, result.Outcome );
        Assert.Same( a.Object, result.Winner );
        _logger.Verify( l => l.LogReachStalemate( It.IsAny<IReadOnlyList<IFighter>>() ), Times.Once );
    }

    [Fact]
    public void Play_NobodyDiesForManyRounds_ReturnsRoundLimitReached()
    {
        Mock<IFighter> a = FighterBuilder.CreateMock( "A", currentHealth: 50, initiative: 10 );
        Mock<IFighter> b = FighterBuilder.CreateMock( "B", currentHealth: 40, initiative: 1 );
        _damage.Setup( d => d.Calculate( It.IsAny<IFighter>(), It.IsAny<IFighter>() ) ).Returns( 1 );
        _selector.Setup( s => s.Pick( a.Object, It.IsAny<IReadOnlyList<IFighter>>() ) ).Returns( b.Object );
        _selector.Setup( s => s.Pick( b.Object, It.IsAny<IReadOnlyList<IFighter>>() ) ).Returns( a.Object );

        BattleResult result = CreateRunner().Play( new[] { a.Object, b.Object } );

        Assert.Equal( BattleOutcome.RoundLimitReached, result.Outcome );
        Assert.Same( a.Object, result.Winner );
    }
}