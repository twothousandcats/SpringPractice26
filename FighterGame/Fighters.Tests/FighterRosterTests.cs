using Fighters.Models.Fighters;
using Fighters.Tests.TestData;

namespace Fighters.Tests;

public class FighterRosterTests
{
    [Fact]
    public void Add_Fighter_AppearsInFighters()
    {
        // Arrange
        FighterRoster sut = new FighterRoster();
        TestFighter testFighter = FighterMother.CreateDefault( "a" );

        // Act
        sut.Add( testFighter );

        // Assert
        Assert.Single( sut.Fighters );
        Assert.Equal( testFighter, sut.Fighters.First() );
    }

    [Fact]
    public void Add_Null_ThrowsArgumentNullException()
    {
        // Arrange
        FighterRoster sut = new FighterRoster();

        // Act, Assert
        Assert.Throws<ArgumentNullException>( () => sut.Add( null! ) );
    }

    [Fact]
    public void RemoveAt_ValidIndex_RemovesThatFighter()
    {
        // Arrange
        FighterRoster sut = new FighterRoster();
        sut.Add( FighterMother.CreateDefault( "First Fighter" ) );
        sut.Add( FighterMother.CreateDefault( "Second Fighter" ) );

        // Act
        sut.RemoveAt( 0 );

        // Assert
        Assert.Equal( new[] { "Second Fighter" }, sut.Fighters.Select( f => f.Name ) );
    }

    [Fact]
    public void RemoveAt_OutOfRangeIndex_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        FighterRoster sut = new FighterRoster();
        sut.Add( FighterMother.CreateDefault() );

        // Act, Assert
        Assert.Throws<ArgumentOutOfRangeException>( () => sut.RemoveAt( 5 ) );
    }

    [Fact]
    public void Clear_NonEmptyRoster_RemovesEverything()
    {
        // Arrange
        FighterRoster sut = new FighterRoster();
        sut.Add( FighterMother.CreateDefault( "a" ) );
        sut.Add( FighterMother.CreateDefault( "b" ) );

        // Act
        sut.Clear();

        // Assert
        Assert.Empty( sut.Fighters );
    }
}