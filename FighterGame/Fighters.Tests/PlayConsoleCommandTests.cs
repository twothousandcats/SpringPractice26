using Fighters.Battle;
using Fighters.Commands;
using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Fighters.Tests.Helpers;
using NUnit.Framework;

namespace Fighters.Tests;

[TestFixture]
public class PlayConsoleCommandTests
{
    private static BattleRunner MakeRunner() =>
        new BattleRunner(
            new TestBattleLogger(),
            new WeakestTargetSelector(),
            new PlainDamageCalculator()
        );

    [Test]
    public void Execute_LessThanTwoFighters_DoesNotRunBattle()
    {
        FakeConsole console = new FakeConsole();
        List<IFighter> arena = [ Utils.CreateFighter( "A" ) ];
        PlayConsoleCommand consoleCommand = new PlayConsoleCommand( arena, MakeRunner(), console );

        consoleCommand.Execute();

        Assert.That( arena, Has.Count.EqualTo( 1 ) );
        Assert.That( console.Output, Has.Some.Contains( "at least 2" ) );
    }

    [Test]
    public void Execute_ClearsArenaAfterBattle()
    {
        FakeConsole console = new FakeConsole();
        List<IFighter> arena =
        [
            Utils.CreateFighter( "Weak" ),
            new Fighter( "Strong", new Orc(), new Mercenary(), new Axe(), new PlateArmor() ),
        ];

        PlayConsoleCommand consoleCommand = new PlayConsoleCommand( arena, MakeRunner(), console );

        consoleCommand.Execute();

        Assert.That( arena, Is.Empty );
    }
}