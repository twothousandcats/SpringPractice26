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
    public class RemoveFighterCommandTests
    {
        [Test]
        public void Execute_ValidIndex_RemovesFighter()
        {
            FakeConsole console = new FakeConsole("1");
            List<IFighter> arena = [Utils.CreateFighter("A"), Utils.CreateFighter("B")];
            RemoveFighterCommand command = new RemoveFighterCommand(arena, console);

            command.Execute();

            Assert.That(arena.Select(f => f.Name), Is.EqualTo(new[] { "B" }));
            Assert.That(console.Output, Has.Some.Contains("Removed"));
        }

        [TestCase("0")]
        [TestCase("3")]
        [TestCase("-1")]
        [TestCase("abc")]
        [TestCase("")]
        public void Execute_InvalidInput_LeavesArenaIntact(string input)
        {
            FakeConsole console = new FakeConsole(input);
            List<IFighter> arena = [Utils.CreateFighter("A"), Utils.CreateFighter("B")];
            RemoveFighterCommand command = new RemoveFighterCommand(arena, console);

            command.Execute();

            Assert.That(arena, Has.Count.EqualTo(2));
            Assert.That(console.Output, Has.Some.Contains("Invalid"));
        }
    }
}
