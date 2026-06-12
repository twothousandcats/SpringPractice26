using Fighters.Commands;
using Fighters.Tests.TestData;
using Fighters.UI;
using Moq;

namespace Fighters.Tests.Commands;

public class CommandLoopTests
{
    [Fact]
    public void Run_KnownCommand_ExecutesIt()
    {
        // Arrange
        TestCommand play = new TestCommand
        {
            Name = "Play"
        };

        CommandRegistry registry = new CommandRegistry();
        registry.Register( play );
        Mock<IGameLoop> gameLoop = new Mock<IGameLoop>();
        gameLoop.SetupSequence( g => g.IsRunning ).Returns( true ).Returns( false );
        Mock<IConsole> console = new Mock<IConsole>();
        console.Setup( c => c.ReadLine() ).Returns( "Play" );
        CommandLoop sut = new CommandLoop( registry, gameLoop.Object, console.Object );

        // Act
        sut.Run();

        // Assert
        Assert.Equal( 1, play.ExecuteCallCount );
    }

    [Fact]
    public void Run_GameLoopNotRunning_DoesNothing()
    {
        // Arrange
        CommandRegistry registry = new CommandRegistry();
        Mock<IGameLoop> gameLoop = new Mock<IGameLoop>();
        gameLoop
            .Setup( g => g.IsRunning )
            .Returns( false );

        Mock<IConsole> console = new Mock<IConsole>();
        CommandLoop sut = new CommandLoop(
            registry,
            gameLoop.Object,
            console.Object
        );

        // Act
        sut.Run();

        // Assert
        console.Verify( c => c.ReadLine(), Times.Never );
    }

    [Fact]
    public void Run_MultipleIterations_ExecutesCommandPerIteration()
    {
        // Arrange
        TestCommand play = new TestCommand
        {
            Name = "Play"
        };

        CommandRegistry registry = new CommandRegistry();
        registry.Register( play );
        Mock<IGameLoop> gameLoop = new Mock<IGameLoop>();
        gameLoop
            .SetupSequence( g => g.IsRunning )
            .Returns( true )
            .Returns( true )
            .Returns( false );

        Mock<IConsole> console = new Mock<IConsole>();
        console
            .SetupSequence( c => c.ReadLine() )
            .Returns( "Play" )
            .Returns( "Play" );

        CommandLoop sut = new CommandLoop( registry, gameLoop.Object, console.Object );

        // Act
        sut.Run();

        // Assert
        Assert.Equal( 2, play.ExecuteCallCount );
    }
}