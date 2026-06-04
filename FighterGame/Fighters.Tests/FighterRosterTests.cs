using Fighters.Models.Fighters;
using Fighters.Tests.TestData;

namespace Fighters.Tests;

public class FighterRosterTests
{
    [Fact]
    public void Add_Fighter_AppearsInFighters()
    {
        FighterRoster roster = new FighterRoster();
        IFighter fighter = FighterBuilder.CreateMock( "A" ).Object;

        roster.Add( fighter );

        Assert.Single( roster.Fighters );
        Assert.Same( fighter, roster.Fighters[ 0 ] );
    }

    [Fact]
    public void Add_Null_ThrowsArgumentNullException()
    {
        FighterRoster roster = new FighterRoster();

        Assert.Throws<ArgumentNullException>( () => roster.Add( null! ) );
    }

    [Fact]
    public void Add_MultipleFighters_PreservesInsertionOrder()
    {
        FighterRoster roster = new FighterRoster();
        roster.Add( FighterBuilder.CreateMock( "A" ).Object );
        roster.Add( FighterBuilder.CreateMock( "B" ).Object );

        Assert.Equal( new[] { "A", "B" }, roster.Fighters.Select( f => f.Name ) );
    }

    [Fact]
    public void RemoveAt_ValidIndex_RemovesThatFighter()
    {
        FighterRoster roster = new FighterRoster();
        roster.Add( FighterBuilder.CreateMock( "A" ).Object );
        roster.Add( FighterBuilder.CreateMock( "B" ).Object );

        roster.RemoveAt( 0 );

        Assert.Equal( new[] { "B" }, roster.Fighters.Select( f => f.Name ) );
    }

    [Fact]
    public void RemoveAt_OutOfRangeIndex_ThrowsArgumentOutOfRangeException()
    {
        FighterRoster roster = new FighterRoster();
        roster.Add( FighterBuilder.CreateMock( "A" ).Object );

        Assert.Throws<ArgumentOutOfRangeException>( () => roster.RemoveAt( 5 ) );
    }

    [Fact]
    public void Clear_NonEmptyRoster_RemovesEverything()
    {
        FighterRoster roster = new FighterRoster();
        roster.Add( FighterBuilder.CreateMock( "A" ).Object );
        roster.Add( FighterBuilder.CreateMock( "B" ).Object );

        roster.Clear();

        Assert.Empty( roster.Fighters );
    }
}