using Fighters.Commands;
using Fighters.Models.Fighters;
using Fighters.Tests.TestData;
using Fighters.UI;
using Moq;

namespace Fighters.Tests.Commands;

public class AddFighterConsoleCommandTests
{
    [Fact]
    public void Execute_Always_AddsCreatedFighterToRosterAndAnnounces()
    {
        FighterRoster roster = new FighterRoster();
        IFighter fighter = FighterBuilder.CreateMock( "Hero" ).Object;
        Mock<IFighterFactory> factory = new Mock<IFighterFactory>();
        factory.Setup( f => f.Create() ).Returns( fighter );
        Mock<IConsole> console = new Mock<IConsole>();
        AddFighterConsoleCommand command = new AddFighterConsoleCommand( roster, factory.Object, console.Object );

        command.Execute();

        Assert.Single( roster.Fighters );
        Assert.Same( fighter, roster.Fighters[ 0 ] );
        factory.Verify( f => f.Create(), Times.Once );
        console.Verify( c => c.WriteLine( It.Is<string>( s => s.Contains( "Hero" ) ) ), Times.Once );
    }
};