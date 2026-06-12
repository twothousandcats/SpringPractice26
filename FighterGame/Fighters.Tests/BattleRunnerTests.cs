using Fighters.Battle;
using Fighters.Models.Fighters;
using Fighters.Tests.TestData;
using Moq;

namespace Fighters.Tests;

public class BattleRunnerTests
{
    [Fact]
    public void Play_NullFighters_ThrowsArgumentNullException()
    {
        // Arrange
        BattleRunner sut = new BattleRunner(
            new Mock<IBattleLogger> ().Object,
            new Mock<ITargetSelector>().Object,
            new Mock<IDamageCalculator>().Object
        );

        // Act, Assert
        Assert.Throws<ArgumentNullException>( () => sut.Play( null! ) );
    }

    [Fact]
    public void Play_FewerThanTwoFighters_ThrowsArgumentException()
    {
        // Arrange
        IFighter solo = FighterMother.CreateDefault();
        BattleRunner sut = new BattleRunner(
            new Mock<IBattleLogger> ().Object,
            new Mock<ITargetSelector>().Object,
            new Mock<IDamageCalculator>().Object
        );

        // Act, Assert
        Assert.Throws<ArgumentException>( () => sut.Play( new IFighter[] { solo } ) );
    }

    [Fact]
    public void Play_NoTargetForAttacker_ReturnsVictoryForActingFighter()
    {
        // Arrange
        TestFighter firstFighter = new TestFighter
        {
            Name = "FirstFighter",
            Initiative = 10
        };

        TestFighter secondFighter = new TestFighter
        {
            Name = "SecondFighter",
            Initiative = 1
        };

        Mock<IBattleLogger> logger = new Mock<IBattleLogger>();
        Mock<ITargetSelector> targetSelector = new Mock<ITargetSelector>();
        targetSelector
            .Setup( selector => selector.Pick( It.IsAny<IFighter>(), It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( ( IFighter? )null );

        BattleRunner sut = new BattleRunner(
            logger.Object,
            targetSelector.Object,
            new Mock<IDamageCalculator>().Object
        );

        // Act
        BattleResult result = sut.Play( new IFighter[] { firstFighter, secondFighter } );

        // Assert
        Assert.Equal( BattleOutcome.Victory, result.Outcome );
        Assert.Same( firstFighter, result.Winner );
        logger.Verify( l => l.LogAnnounceRound( 1 ), Times.Once );
        logger.Verify( l => l.LogFighterWon( firstFighter ), Times.AtLeastOnce );
    }

    [Fact]
    public void Play_HigherInitiative_ActsFirst()
    {
        // Arrange
        TestFighter slow = new TestFighter
        {
            Name = "Slow",
            Initiative = 1
        };

        TestFighter fast = new TestFighter
        {
            Name = "Fast",
            Initiative = 10
        };

        Mock<ITargetSelector> selector = new Mock<ITargetSelector>();
        selector
            .Setup( s => s.Pick( It.IsAny<IFighter>(), It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( ( IFighter? )null );

        BattleRunner sut = new BattleRunner(
            new Mock<IBattleLogger>().Object,
            selector.Object,
            new Mock<IDamageCalculator>().Object
        );

        // Act
        BattleResult result = sut.Play( new IFighter[] { slow, fast } );

        // Assert
        Assert.Same( fast, result.Winner );
    }

    [Fact]
    public void Play_AttackerHitsTarget_AppliesDamageAndLogsAttack()
    {
        // Arrange
        TestFighter attacker = new TestFighter
        {
            Name = "Attacker",
            Initiative = 10
        };

        TestFighter defender = new TestFighter
        {
            Name = "Defender",
            Initiative = 1,
            CurrentHealth = 100
        };

        Mock<IBattleLogger> logger = new Mock<IBattleLogger>();
        Mock<ITargetSelector> targetSelector = new Mock<ITargetSelector>();
        targetSelector
            .SetupSequence( s => s.Pick( It.IsAny<IFighter>(), It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( defender )
            .Returns( ( IFighter? )null );

        Mock<IDamageCalculator> damageCalculator = new Mock<IDamageCalculator>();
        damageCalculator
            .Setup( calculator => calculator.Calculate( attacker, defender ) )
            .Returns( 30 );

        BattleRunner sut = new BattleRunner(
            logger.Object,
            targetSelector.Object,
            damageCalculator.Object
        );

        // Act
        sut.Play( new IFighter[] { attacker, defender } );

        // Assert
        Assert.Equal( 70, defender.CurrentHealth );
        logger.Verify( l => l.LogPerformAttack( attacker, defender, 30 ), Times.Once );
    }

    [Fact]
    public void Play_TargetDies_LogsFighterDied()
    {
        // Arrange
        TestFighter attacker = new TestFighter
        {
            Name = "Attacker",
            Initiative = 10
        };

        TestFighter defender = new TestFighter
        {
            Name = "Defender",
            Initiative = 1,
            CurrentHealth = 1
        };

        Mock<IBattleLogger> logger = new Mock<IBattleLogger>();
        Mock<ITargetSelector> targetSelector = new Mock<ITargetSelector>();
        targetSelector
            .SetupSequence( selector => selector.Pick( attacker, It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( defender )
            .Returns( ( IFighter? )null );

        Mock<IDamageCalculator> damageCalculator = new Mock<IDamageCalculator>();
        damageCalculator
            .Setup( calculator => calculator.Calculate( It.IsAny<IFighter>(), It.IsAny<IFighter>() ) )
            .Returns( 100 );

        BattleRunner sut = new BattleRunner(
            logger.Object,
            targetSelector.Object,
            damageCalculator.Object
        );

        // Act
        BattleResult result = sut.Play( new IFighter[] { attacker, defender } );

        // Assert
        logger.Verify( l => l.LogFighterDied( defender ), Times.Once );
        Assert.Equal( BattleOutcome.Victory, result.Outcome );
        Assert.Same( attacker, result.Winner );
    }

    [Fact]
    public void Play_NobodyDealsDamage_ReturnsStalemateWithStrongestSurvivor()
    {
        // Arrange
        TestFighter firstFighter = new TestFighter
        {
            Name = "FirstFighter",
            Initiative = 10,
            CurrentHealth = 100
        };

        TestFighter secondFighter = new TestFighter
        {
            Name = "SecondFighter",
            Initiative = 1,
            CurrentHealth = 90
        };

        Mock<IBattleLogger> logger = new Mock<IBattleLogger>();
        Mock<ITargetSelector> targetSelector = new Mock<ITargetSelector>();
        targetSelector
            .Setup( selector => selector.Pick( firstFighter, It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( secondFighter );

        targetSelector
            .Setup( selector => selector.Pick( secondFighter, It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( firstFighter );

        Mock<IDamageCalculator> damageCalculator = new Mock<IDamageCalculator>();
        damageCalculator
            .Setup( calculator => calculator.Calculate( It.IsAny<IFighter>(), It.IsAny<IFighter>() ) )
            .Returns( 0 );

        BattleRunner sut = new BattleRunner(
            logger.Object,
            targetSelector.Object,
            damageCalculator.Object
        );

        // Act
        BattleResult result = sut.Play( new IFighter[] { firstFighter, secondFighter } );

        // Assert
        Assert.Equal( BattleOutcome.Stalemate, result.Outcome );
        Assert.Same( firstFighter, result.Winner );
        logger.Verify( l => l.LogReachStalemate( It.IsAny<IReadOnlyList<IFighter>>() ), Times.Once );
    }

    [Fact]
    public void Play_NobodyDiesForManyRounds_ReturnsRoundLimitReached()
    {
        // Arrange
        TestFighter firstFighter = new TestFighter
        {
            Name = "FirstFighter",
            Initiative = 10,
            CurrentHealth = 100
        };

        TestFighter secondFighter = new TestFighter
        {
            Name = "SecondFighter",
            Initiative = 1,
            CurrentHealth = 90
        };

        Mock<ITargetSelector> targetSelector = new Mock<ITargetSelector>();
        targetSelector
            .Setup( selector => selector.Pick( firstFighter, It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( secondFighter );

        targetSelector
            .Setup( selector => selector.Pick( secondFighter, It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( firstFighter );

        Mock<IDamageCalculator> damageCalculator = new Mock<IDamageCalculator>();
        damageCalculator
            .Setup( calculator => calculator.Calculate( It.IsAny<IFighter>(), It.IsAny<IFighter>() ) )
            .Returns( 1 );

        BattleRunner sut = new BattleRunner(
            new Mock<IBattleLogger>().Object,
            targetSelector.Object,
            damageCalculator.Object
        );

        // Act
        BattleResult result = sut.Play( new IFighter[] { firstFighter, secondFighter } );

        // Assert
        Assert.Equal( BattleOutcome.RoundLimitReached, result.Outcome );
        Assert.Same( firstFighter, result.Winner );
    }
}