using Fighters.Commands;
using Fighters.Models.Fighters;
using Fighters.Tests.Helpers;
using NUnit.Framework;

namespace Fighters.Tests;

[TestFixture]
public class RemoveFighterConsoleCommandTests
{
    [Test]
    public void Execute_ValidIndex_RemovesFighter()
    {
        FakeConsole console = new FakeConsole( "1" );
        List<IFighter> arena = [ Utils.CreateFighter( "A" ), Utils.CreateFighter( "B" ) ];
        RemoveFighterConsoleCommand consoleCommand = new RemoveFighterConsoleCommand( arena, console );

        consoleCommand.Execute();

        Assert.That( arena.Select( f => f.Name ), Is.EqualTo( new[] { "B" } ) );
        Assert.That( console.Output, Has.Some.Contains( "Removed" ) );
    }

    [TestCase( "0" )]
    [TestCase( "3" )]
    [TestCase( "-1" )]
    [TestCase( "abc" )]
    [TestCase( "" )]
    public void Execute_InvalidInput_LeavesArenaIntact( string input )
    {
        FakeConsole console = new FakeConsole( input );
        List<IFighter> arena = [ Utils.CreateFighter( "A" ), Utils.CreateFighter( "B" ) ];
        RemoveFighterConsoleCommand consoleCommand = new RemoveFighterConsoleCommand( arena, console );

        consoleCommand.Execute();

        Assert.That( arena, Has.Count.EqualTo( 2 ) );
        Assert.That( console.Output, Has.Some.Contains( "Invalid" ) );
    }
}