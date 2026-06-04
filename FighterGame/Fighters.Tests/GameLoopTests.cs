namespace Fighters.Tests;

public class GameLoopTests
{
    [Fact]
    public void Constructor_NewInstance_IsRunning()
    {
        Assert.True( new GameLoop().IsRunning );
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