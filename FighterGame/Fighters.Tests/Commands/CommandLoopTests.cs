using Fighters.Commands;
using Fighters.UI;
using Moq;

namespace Fighters.Tests.Commands;

public class CommandLoopTests
{
    private readonly Mock<IGameLoop> _gameLoop = new();

    private readonly Mock<IConsole> _console = new();

    private readonly CommandRegistry _registry = new();

    private CommandLoop CreateLoop() => new( _registry, _gameLoop.Object, _console.Object );

    private void RunsExactlyOneIteration() => _gameLoop
        .SetupSequence( gameLoop => gameLoop.IsRunning )
        .Returns( true ).Returns( false );

    [Fact]
    public void Run_KnownCommand_ExecutesIt()
    {
        Mock<IConsoleCommand> command = new Mock<IConsoleCommand>();
        command.SetupGet( c => c.Name ).Returns( "Play" );
        command.SetupGet( c => c.Description ).Returns( "play" );
        _registry.Register( command.Object );
        RunsExactlyOneIteration();
        _console.Setup( c => c.ReadLine() ).Returns( "Play" );

        CreateLoop().Run();

        command.Verify( c => c.Execute(), Times.Once );
    }

    [Fact]
    public void Run_UnknownCommand_PrintsUnknownMessage()
    {
        RunsExactlyOneIteration();
        _console.Setup( c => c.ReadLine() ).Returns( "nope" );

        CreateLoop().Run();

        _console.Verify( c => c.WriteLine( "Unknown command" ), Times.Once );
    }

    [Fact]
    public void Run_EmptyInput_PrintsUnknownMessage()
    {
        RunsExactlyOneIteration();
        _console.Setup( c => c.ReadLine() ).Returns( string.Empty );

        CreateLoop().Run();

        _console.Verify( c => c.WriteLine( "Unknown command" ), Times.Once );
    }

    [Fact]
    public void Run_GameLoopNotRunning_DoesNothing()
    {
        _gameLoop.SetupGet( g => g.IsRunning ).Returns( false );

        CreateLoop().Run();

        _console.Verify( c => c.ReadLine(), Times.Never );
    }
}