namespace Fighters.Tests;

public class GameLoopTests
{
    [Fact]
    public void Constructor_Always_IsRunning()
    {
        // Arrange
        GameLoop sut = new GameLoop();

        // Act
        bool isRunning = sut.IsRunning;

        // Assert
        Assert.True( isRunning );
    }

    [Fact]
    public void RequestStop_Called_StopsRunning()
    {
        // Arrange
        GameLoop sut = new GameLoop();

        // Act
        sut.RequestStop();

        // Assert
        Assert.False( sut.IsRunning );
    }

    [Fact]
    public void RequestStop_CalledTwice_RemainsStopped()
    {
        // Arrange
        GameLoop sut = new GameLoop();

        // Act
        sut.RequestStop();
        sut.RequestStop();

        // Assert
        Assert.False( sut.IsRunning );
    }
}