using Fighters.Commands;
using Fighters.Tests.Helpers;
using NUnit.Framework;

namespace Fighters.Tests;

[TestFixture]
public class HelpConsoleCommandTests
{
    [Test]
    public void Execute_PrintsAllRegisteredCommands()
    {
        FakeConsole console = new FakeConsole();
        CommandRegistry registry = new CommandRegistry();
        registry.Register( new StubConsoleCommand( "Play", "Starts the battle" ) );
        registry.Register( new StubConsoleCommand( "Exit", "Exits the game" ) );
        HelpConsoleCommand helpConsole = new HelpConsoleCommand( registry, console );
        registry.Register( helpConsole );

        helpConsole.Execute();

        Assert.That( console.Output, Has.Some.Contains( "Play" ).And.Contain( "Starts" ) );
        Assert.That( console.Output, Has.Some.Contains( "Exit" ).And.Contain( "Exits" ) );
        Assert.That( console.Output, Has.Some.Contains( "Help" ) );
    }
}