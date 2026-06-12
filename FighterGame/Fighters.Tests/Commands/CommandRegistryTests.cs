using Fighters.Commands;
using Fighters.Tests.TestData;

namespace Fighters.Tests.Commands;

public class CommandRegistryTests
{
    [Fact]
    public void TryGet_RegisteredCommand_ReturnsTrueAndSameInstance()
    {
        // Arrange
        CommandRegistry sut = new CommandRegistry();
        IConsoleCommand play = new TestCommand
        {
            Name = "Play"
        };

        sut.Register( play );

        // Act
        bool found = sut.TryGet( "Play", out IConsoleCommand resolved );

        // Assert
        Assert.True( found );
        Assert.Same( play, resolved );
    }

    [Fact]
    public void TryGet_DifferentCase_ReturnsTrue()
    {
        // Arrange
        CommandRegistry sut = new CommandRegistry();
        sut.Register(
            new TestCommand
            {
                Name = "Play"
            }
        );

        // Act
        bool found = sut.TryGet( "Play", out _ );

        // Assert
        Assert.True( found );
    }

    [Fact]
    public void Register_SameName_OverwritesPrevious()
    {
        // Arrange
        CommandRegistry sut = new CommandRegistry();
        TestCommand first = new TestCommand
        {
            Name = "Play",
            Description = "first"
        };

        TestCommand second = new TestCommand
        {
            Name = "Play",
            Description = "second"
        };

        // Act
        sut.Register( first );
        sut.Register( second );
        sut.TryGet( "Play", out IConsoleCommand resolved );

        // Arrange
        Assert.Same( second, resolved );
    }
}