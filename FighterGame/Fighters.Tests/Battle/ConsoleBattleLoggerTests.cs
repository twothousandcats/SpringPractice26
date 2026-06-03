using Fighters.Battle;
using Fighters.Models.Fighters;
using Fighters.Tests.TestData;
using Fighters.UI;
using Moq;

namespace Fighters.Tests.Battle;

public class ConsoleBattleLoggerTests
{
    private readonly Mock<IConsole> _console = new Mock<IConsole>();

    private readonly ConsoleBattleLogger _logger;

    public ConsoleBattleLoggerTests() => _logger = new ConsoleBattleLogger( _console.Object );

    private void VerifyWritten( Func<string, bool> predicate ) => _console.Verify(
        console => console.WriteLine(
            It.Is<string>( s => predicate( s ) )
        ),
        Times.Once
    );

    [Fact]
    public void LogAnnounceRound_Always_WritesRoundNumber()
    {
        _logger.LogAnnounceRound( 7 );

        VerifyWritten( s => s.Contains( "7" ) );
    }

    [Fact]
    public void LogPerformAttack_Always_WritesAttackerTargetAndDamage()
    {
        IFighter attacker = FighterBuilder.CreateMock( "Alice" ).Object;
        IFighter target = FighterBuilder.CreateMock( "Bob" ).Object;

        _logger.LogPerformAttack( attacker, target, 12 );

        VerifyWritten( s => s.Contains( "Alice" ) && s.Contains( "Bob" ) && s.Contains( "12" ) );
    }

    [Fact]
    public void LogFighterDied_Always_AnnouncesDeath()
    {
        _logger.LogFighterDied( FighterBuilder.CreateMock( "Alice" ).Object );

        VerifyWritten( s => s.Contains( "Alice" ) && s.Contains( "dies" ) );
    }

    [Fact]
    public void LogFighterWon_Always_AnnouncesWinner()
    {
        _logger.LogFighterWon( FighterBuilder.CreateMock( "Alice" ).Object );

        VerifyWritten( s => s.Contains( "Alice" ) && s.Contains( "wins" ) );
    }

    [Fact]
    public void LogReachStalemate_Always_ListsAllSurvivors()
    {
        IFighter a = FighterBuilder.CreateMock( "Alice" ).Object;
        IFighter b = FighterBuilder.CreateMock( "Bob" ).Object;

        _logger.LogReachStalemate( new[] { a, b } );

        VerifyWritten( s => s.Contains( "Alice" ) && s.Contains( "Bob" ) && s.Contains( "Stalemate" ) );
    }
}