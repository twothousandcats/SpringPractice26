using Fighters.Commands;
using Fighters.UI;
using Moq;

namespace Fighters.Tests.Commands;

public class HelpConsoleCommandTests
{
    [Fact]
    public void Execute_Always_PrintsEveryRegisteredCommand()
    {
        CommandRegistry registry = new CommandRegistry();
        Mock<IConsoleCommand> play = new Mock<IConsoleCommand>();
        play.SetupGet( c => c.Name ).Returns( "Play" );
        play.SetupGet( c => c.Description ).Returns( "Starts the battle" );
        Mock<IConsoleCommand> exit = new Mock<IConsoleCommand>();
        exit.SetupGet( c => c.Name ).Returns( "Exit" );
        exit.SetupGet( c => c.Description ).Returns( "Exits the game" );
        registry.Register( play.Object );
        registry.Register( exit.Object );
        Mock<IConsole> console = new Mock<IConsole>();

        new HelpConsoleCommand( registry, console.Object ).Execute();

        console.Verify(
            c => c.WriteLine( It.Is<string>( s => s.Contains( "Play" ) && s.Contains( "Starts the battle" ) ) ),
            Times.Once
        );

        console.Verify(
            c => c.WriteLine( It.Is<string>( s => s.Contains( "Exit" ) && s.Contains( "Exits the game" ) ) ),
            Times.Once
        );
    }
}