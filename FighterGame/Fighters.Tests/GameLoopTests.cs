namespace Fighters.Tests;

public class GameLoopTests
{
    [Fact]
    public void Constructor_NewInstance_IsRunning()
    {
        GameLoop gameLoop = new GameLoop();

        bool isRunning = gameLoop.IsRunning;

        Assert.True( isRunning );
    }

    [Fact]
    public void RequestStop_Called_StopsRunning()
    {
        GameLoop gameLoop = new GameLoop();

        gameLoop.RequestStop();

        Assert.False( gameLoop.IsRunning );
    }

    [Fact]
    public void RequestStop_CalledTwice_RemainsStopped()
    {
        GameLoop gameLoop = new GameLoop();

        gameLoop.RequestStop();
        gameLoop.RequestStop();

        Assert.False( gameLoop.IsRunning );
    }
}