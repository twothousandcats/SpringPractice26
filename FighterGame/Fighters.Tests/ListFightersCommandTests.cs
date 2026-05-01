using Fighters.Commands;
using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Fighters.Tests.Helpers;
using NUnit.Framework;

namespace Fighters.Tests
{
    [TestFixture]
    public class ListFightersCommandTests
    {
        [Test]
        public void Execute_EmptyArena_PrintsEmptyMessage()
        {
            FakeConsole console = new FakeConsole();
            ListFightersCommand command = new ListFightersCommand([], console);

            command.Execute();

            Assert.That(console.Output, Has.Some.Contains("empty"));
        }

        [Test]
        public void Execute_NonEmptyArena_PrintsNumberedRows()
        {
            FakeConsole console = new FakeConsole();
            List<IFighter> arena =
            [
                new Fighter("A", new Human(), new Knight(), new Fists(), new NoArmor()),
                new Fighter("B", new Orc(), new Mage(), new Sword(), new PlateArmor()),
            ];
            ListFightersCommand command = new ListFightersCommand(arena, console);

            command.Execute();

            Assert.That(console.Output, Has.Some.StartsWith("1. ").And.Contain("A"));
            Assert.That(console.Output, Has.Some.StartsWith("2. ").And.Contain("B"));
        }
    }
}
