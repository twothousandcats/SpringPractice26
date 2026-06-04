using Fighters.Battle;
using Fighters.Commands;
using Fighters.Models.Fighters;
using Fighters.Tests.TestData;
using Fighters.UI;
using Moq;

namespace Fighters.Tests.Commands;

public class PlayConsoleCommandTests
{
    private readonly Mock<IBattleRunner> _runner = new Mock<IBattleRunner>();

    private readonly Mock<IConsole> _console = new Mock<IConsole>();

    private static FighterRoster CreateRosterWith( int count )
    {
        FighterRoster roster = new FighterRoster();
        for ( int i = 0; i < count; i++ )
        {
            roster.Add( FighterBuilder.CreateMock( $"F{i}" ).Object );
        }

        return roster;
    }

    private PlayConsoleCommand Create( FighterRoster roster ) => new PlayConsoleCommand(
        roster,
        _runner.Object,
        _console.Object
    );

    private void RunnerReturns( BattleResult result ) =>
        _runner
            .Setup( runner => runner.Play( It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( result );

    [Fact]
    public void Execute_FewerThanTwoFighters_DoesNotRunBattle()
    {
        FighterRoster roster = CreateRosterWith( 1 );

        Create( roster ).Execute();

        _runner.Verify( runner => runner.Play( It.IsAny<IReadOnlyList<IFighter>>() ), Times.Never );
        _console.Verify( console => console.WriteLine( It.Is<string>( s => s.Contains( "at least 2" ) ) ), Times.Once );
        Assert.Single( roster.Fighters );
    }

    [Fact]
    public void Execute_EnoughFighters_RunsBattleAndClearsRoster()
    {
        FighterRoster roster = CreateRosterWith( 2 );
        RunnerReturns( BattleResult.Victory( FighterBuilder.CreateMock( "Champ" ).Object ) );

        Create( roster ).Execute();

        _runner.Verify( runner => runner.Play( It.IsAny<IReadOnlyList<IFighter>>() ), Times.Once );
        Assert.Empty( roster.Fighters );
    }

    [Fact]
    public void Execute_Victory_AnnouncesWinner()
    {
        FighterRoster roster = CreateRosterWith( 2 );
        RunnerReturns( BattleResult.Victory( FighterBuilder.CreateMock( "Champ" ).Object ) );

        Create( roster ).Execute();

        _console.Verify(
            console => console.WriteLine(
                It.Is<string>( s => s.Contains( "Champ" ) && s.Contains( "wins the battle" ) )
            ),
            Times.Once
        );
    }

    [Fact]
    public void Execute_Stalemate_AnnouncesStalemate()
    {
        FighterRoster roster = CreateRosterWith( 2 );
        RunnerReturns( BattleResult.Stalemate( FighterBuilder.CreateMock( "Champ" ).Object ) );

        Create( roster ).Execute();

        _console.Verify(
            console => console.WriteLine( It.Is<string>( s => s.Contains( "Stalemate" ) && s.Contains( "Champ" ) ) ),
            Times.Once
        );
    }

    [Fact]
    public void Execute_RoundLimitReached_AnnouncesRoundLimit()
    {
        FighterRoster roster = CreateRosterWith( 2 );
        RunnerReturns( BattleResult.RoundLimitReached( FighterBuilder.CreateMock( "Champ" ).Object ) );

        Create( roster ).Execute();

        _console.Verify(
            console => console.WriteLine( It.Is<string>( s => s.Contains( "Round limit" ) && s.Contains( "Champ" ) ) ),
            Times.Once
        );
    }
}