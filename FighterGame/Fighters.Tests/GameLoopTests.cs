using NUnit.Framework;

namespace Fighters.Tests;

[TestFixture]
public class GameLoopTests
{
    [Test]
    public void NewGameLoop_ShouldStop_IsFalse()
    {
        IGameLoop gameLoop = new GameLoop();

        bool isRunning = gameLoop.IsRunning;

        Assert.That( isRunning, Is.True );
    }

    [Test]
    public void RequestStop_SetsShouldStop()
    {
        IGameLoop gameLoop = new GameLoop();

        gameLoop.RequestStop();
        bool isRunning = gameLoop.IsRunning;

        Assert.That( isRunning, Is.False );
    }

    [Test]
    public void RequestStop_IsIdempotent()
    {
        IGameLoop gameLoop = new GameLoop();

        gameLoop.RequestStop();
        gameLoop.RequestStop();
        bool isRunning = gameLoop.IsRunning;

        Assert.That( isRunning, Is.False );
    }
}