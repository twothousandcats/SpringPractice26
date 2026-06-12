using Fighters.Commands;
using Fighters.Tests.TestData;
using Fighters.UI;
using Moq;

namespace Fighters.Tests.Commands;

public class AddFighterConsoleCommandTests
{
    [Fact]
    public void Execute_Always_AddsCreatedFighterToRosterAndAnnounces()
    {
        // Arrange
        FighterRoster roster = new FighterRoster();
        TestFighter fighter = FighterMother.CreateDefault( "Hero" );
        Mock<IFighterFactory> factory = new Mock<IFighterFactory>();
        factory
            .Setup( f => f.Create() )
            .Returns( fighter );

        AddFighterConsoleCommand command = new AddFighterConsoleCommand(
            roster,
            factory.Object,
            new Mock<IConsole>().Object
        );

        // Act
        command.Execute();

        // Assert
        Assert.Single( roster.Fighters );
        Assert.Same( fighter, roster.Fighters[ 0 ] );
        factory.Verify( f => f.Create(), Times.Once );
    }
}