using Fighters.Battle;
using Fighters.Tests.Helpers;
using NUnit.Framework;

namespace Fighters.Tests;

[TestFixture]
public class ConsoleBattleLoggerTests
{
    [Test]
    public void RoundStarted_PrintsRoundNumber()
    {
        FakeConsole console = new FakeConsole();
        ConsoleBattleLogger logger = new ConsoleBattleLogger( console );

        logger.RoundStarted( 7 );

        Assert.That( console.Output, Has.Some.Contains( "7" ) );
    }

    [Test]
    public void AttackPerformed_FormatsAttackerTargetAndDamage()
    {
        FakeConsole console = new FakeConsole();
        ConsoleBattleLogger logger = new ConsoleBattleLogger( console );

        logger.AttackPerformed( Utils.CreateFighter( "A" ), Utils.CreateFighter( "B" ), 12 );

        Assert.That( console.Output, Has.Some.Contains( "A" ).And.Contain( "12" ).And.Contain( "B" ) );
    }

    [Test]
    public void FighterDied_AnnouncesDeath()
    {
        FakeConsole console = new FakeConsole();
        ConsoleBattleLogger logger = new ConsoleBattleLogger( console );

        logger.FighterDied( Utils.CreateFighter( "A" ) );

        Assert.That( console.Output, Has.Some.Contains( "A" ).And.Contain( "dies" ) );
    }

    [Test]
    public void FighterWon_AnnouncesWinner()
    {
        FakeConsole console = new FakeConsole();
        ConsoleBattleLogger logger = new ConsoleBattleLogger( console );

        logger.FighterWon( Utils.CreateFighter( "A" ) );

        Assert.That( console.Output, Has.Some.Contains( "A" ).And.Contain( "wins" ) );
    }

    [Test]
    public void StalemateReached_ListsSurvivors()
    {
        FakeConsole console = new FakeConsole();
        ConsoleBattleLogger logger = new ConsoleBattleLogger( console );

        logger.StalemateReached( [ Utils.CreateFighter( "A" ), Utils.CreateFighter( "B" ) ] );

        Assert.That( console.Output, Has.Some.Contains( "A" ).And.Contain( "B" ).And.Contain( "Stalemate" ) );
    }
}