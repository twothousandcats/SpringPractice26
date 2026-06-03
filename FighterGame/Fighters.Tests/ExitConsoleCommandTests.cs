using Fighters.Commands;
using NUnit.Framework;

namespace Fighters.Tests;

[TestFixture]
public class ExitConsoleCommandTests
{
    [Test]
    public void Execute_RequestsGameLoopStop()
    {
        IGameLoop gameLoop = new GameLoop();
        ExitConsoleCommand consoleCommand = new ExitConsoleCommand( gameLoop );

        consoleCommand.Execute();

        Assert.That( gameLoop.IsRunning, Is.False );
    }
}