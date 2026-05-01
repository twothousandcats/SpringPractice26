using Fighters.Commands;
using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Fighters.Tests.Helpers;
using Fighters.UI;
using NUnit.Framework;

namespace Fighters.Tests
{
    [TestFixture]
    public class AddFighterCommandTests
    {
        private class StubFactory : IFighterFactory
        {
            private readonly IFighter _fighter;
            public StubFactory(IFighter fighter) => _fighter = fighter;
            public IFighter Create() => _fighter;
        }

        [Test]
        public void Execute_AddsFighterToArenaAndAnnounces()
        {
            FakeConsole console = new FakeConsole();
            List<IFighter> arena = [];
            IFighter fighter = Utils.CreateFighter("Hero");
            AddFighterCommand command = new AddFighterCommand(arena, new StubFactory(fighter), console);

            command.Execute();

            Assert.That(arena, Has.Count.EqualTo(1));
            Assert.That(arena[0], Is.SameAs(fighter));
            Assert.That(console.Output, Has.Some.Contains("Hero"));
        }
    }
}
