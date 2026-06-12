using Fighters.Battle;
using Fighters.Commands;
using Fighters.Models.Fighters;
using Fighters.Tests.TestData;
using Fighters.UI;
using Moq;

namespace Fighters.Tests.Commands;

public class PlayConsoleCommandTests
{
    [Fact]
    public void Execute_FewerThanTwoFighters_DoesNotRunBattle()
    {
        // Arrange
        FighterRoster roster = new FighterRoster();
        roster.Add( FighterMother.CreateDefault( "Solo" ) );
        Mock<IBattleRunner> runner = new Mock<IBattleRunner>();
        PlayConsoleCommand sut = new PlayConsoleCommand(
            roster,
            runner.Object,
            new Mock<IConsole>().Object
        );

        // Act
        sut.Execute();

        // Assert
        runner.Verify( r => r.Play( It.IsAny<IReadOnlyList<IFighter>>() ), Times.Never );
        Assert.Single( roster.Fighters );
    }

    [Fact]
    public void Execute_EnoughFighters_RunsBattleAndClearsRoster()
    {
        // Arrange
        FighterRoster roster = new FighterRoster();
        roster.Add( FighterMother.CreateDefault( "A" ) );
        roster.Add( FighterMother.CreateDefault( "B" ) );
        Mock<IBattleRunner> runner = new Mock<IBattleRunner>();
        runner
            .Setup( r => r.Play( It.IsAny<IReadOnlyList<IFighter>>() ) )
            .Returns( BattleResult.Victory( FighterMother.CreateDefault( "Champ" ) ) );

        PlayConsoleCommand sut = new PlayConsoleCommand(
            roster,
            runner.Object,
            new Mock<IConsole>().Object
        );

        // Act
        sut.Execute();

        // Assert
        runner.Verify( r => r.Play( It.IsAny<IReadOnlyList<IFighter>>() ), Times.Once );
        Assert.Empty( roster.Fighters );
    }
}