using Fighters.Commands;
using Fighters.Tests.Helpers;
using NUnit.Framework;

namespace Fighters.Tests;

[TestFixture]
public class CommandRegistryTests
{
    [Test]
    public void TryGet_RegisteredCommand_ReturnsTrueAndInstance()
    {
        CommandRegistry registry = new CommandRegistry();
        StubConsoleCommand consoleCommand = new StubConsoleCommand( "Play", "play" );
        registry.Register( consoleCommand );

        bool found = registry.TryGet( "Play", out IConsoleCommand resolved );

        Assert.That( found, Is.True );
        Assert.That( resolved, Is.SameAs( consoleCommand ) );
    }

    [Test]
    public void TryGet_IsCaseInsensitive()
    {
        CommandRegistry registry = new CommandRegistry();
        registry.Register( new StubConsoleCommand( "Play", "play" ) );

        bool found = registry.TryGet( "play", out IConsoleCommand _ );

        Assert.That( found, Is.True );
    }

    [Test]
    public void TryGet_UnknownCommand_ReturnsFalse()
    {
        CommandRegistry registry = new CommandRegistry();

        bool found = registry.TryGet( "nope", out IConsoleCommand _ );

        Assert.That( found, Is.False );
    }

    [Test]
    public void Register_SameName_OverwritesPrevious()
    {
        CommandRegistry registry = new CommandRegistry();
        StubConsoleCommand first = new StubConsoleCommand( "Play", "first" );
        StubConsoleCommand second = new StubConsoleCommand( "Play", "second" );

        registry.Register( first );
        registry.Register( second );
        registry.TryGet( "Play", out IConsoleCommand resolved );

        Assert.That( resolved, Is.SameAs( second ) );
    }

    [Test]
    public void All_ReturnsRegisteredCommands()
    {
        CommandRegistry registry = new CommandRegistry();
        registry.Register( new StubConsoleCommand( "Play", "p" ) );
        registry.Register( new StubConsoleCommand( "Exit", "e" ) );

        Assert.That(
            registry.All.Select( c => c.Name ),
            Is.EquivalentTo( new[] { "Play", "Exit" } )
        );
    }
}