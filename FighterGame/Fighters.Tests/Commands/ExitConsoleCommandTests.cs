using Fighters.Commands;
using Moq;

namespace Fighters.Tests.Commands;

public class ExitConsoleCommandTests
{
    [Fact]
    public void Execute_Always_RequestsGameLoopStop()
    {
        // Arrange
        Mock<IGameLoop> gameLoop = new Mock<IGameLoop>();
        ExitConsoleCommand sut = new ExitConsoleCommand( gameLoop.Object );

        // Act
        sut.Execute();

        // Assert
        gameLoop.Verify( g => g.RequestStop(), Times.Once );
    }
}