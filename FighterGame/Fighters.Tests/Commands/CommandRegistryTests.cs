using Fighters.Commands;
using Moq;

namespace Fighters.Tests.Commands;

public class CommandRegistryTests
{
    private static IConsoleCommand CreateCommand( string name, string description = "desc" )
    {
        Mock<IConsoleCommand> command = new Mock<IConsoleCommand>();
        command.SetupGet( c => c.Name ).Returns( name );
        command.SetupGet( c => c.Description ).Returns( description );

        return command.Object;
    }

    [Fact]
    public void TryGet_RegisteredCommand_ReturnsTrueAndSameInstance()
    {
        CommandRegistry registry = new CommandRegistry();
        IConsoleCommand play = CreateCommand( "Play" );
        registry.Register( play );

        bool found = registry.TryGet( "Play", out IConsoleCommand resolved );

        Assert.True( found );
        Assert.Same( play, resolved );
    }

    [Fact]
    public void TryGet_DifferentCase_ReturnsTrue()
    {
        CommandRegistry registry = new CommandRegistry();
        registry.Register( CreateCommand( "Play" ) );

        Assert.True( registry.TryGet( "play", out _ ) );
    }

    [Fact]
    public void TryGet_UnknownName_ReturnsFalse()
    {
        CommandRegistry registry = new CommandRegistry();

        Assert.False( registry.TryGet( "nope", out _ ) );
    }

    [Fact]
    public void Register_SameName_OverwritesPrevious()
    {
        CommandRegistry registry = new CommandRegistry();
        IConsoleCommand first = CreateCommand( "Play", "first" );
        IConsoleCommand second = CreateCommand( "Play", "second" );

        registry.Register( first );
        registry.Register( second );
        registry.TryGet( "Play", out IConsoleCommand resolved );

        Assert.Same( second, resolved );
    }

    [Fact]
    public void All_Always_ReturnsAllRegisteredCommands()
    {
        CommandRegistry registry = new CommandRegistry();
        registry.Register( CreateCommand( "Play" ) );
        registry.Register( CreateCommand( "Exit" ) );

        Assert.Equal(
            new[] { "Exit", "Play" },
            registry.All.Select( c => c.Name ).OrderBy( n => n )
        );
    }
}