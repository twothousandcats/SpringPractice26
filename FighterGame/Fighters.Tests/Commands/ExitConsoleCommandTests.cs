using Fighters.Commands;
using Moq;

namespace Fighters.Tests.Commands;

public class ExitConsoleCommandTests
{
    [Fact]
    public void Execute_Always_RequestsGameLoopStop()
    {
        Mock<IGameLoop> gameLoop = new Mock<IGameLoop>();

        new ExitConsoleCommand( gameLoop.Object ).Execute();

        gameLoop.Verify( g => g.RequestStop(), Times.Once );
    }
}