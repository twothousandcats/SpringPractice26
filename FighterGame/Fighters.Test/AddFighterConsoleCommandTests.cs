using Fighters.Commands;
using Fighters.Models.Fighters;
using Fighters.Tests.Helpers;
using Fighters.UI;
using NUnit.Framework;

namespace Fighters.Tests;

[TestFixture]
public class AddFighterConsoleCommandTests
{
    private class StubFactory : IFighterFactory
    {
        private readonly IFighter _fighter;

        public StubFactory( IFighter fighter ) => _fighter = fighter;
        public IFighter Create() => _fighter;
    }

    [Test]
    public void Execute_AddsFighterToArenaAndAnnounces()
    {
        FakeConsole console = new FakeConsole();
        List<IFighter> arena = [ ];
        IFighter fighter = Utils.CreateFighter( "Hero" );
        AddFighterConsoleCommand consoleCommand = new AddFighterConsoleCommand(
            arena, new StubFactory( fighter ), console
        );

        consoleCommand.Execute();

        Assert.That( arena, Has.Count.EqualTo( 1 ) );
        Assert.That( arena[ 0 ], Is.SameAs( fighter ) );
        Assert.That( console.Output, Has.Some.Contains( "Hero" ) );
    }
}