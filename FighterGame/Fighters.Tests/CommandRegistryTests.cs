using Fighters.Commands;
using Fighters.Tests.Helpers;
using NUnit.Framework;

namespace Fighters.Tests
{
    [TestFixture]
    public class CommandRegistryTests
    {
        [Test]
        public void TryGet_RegisteredCommand_ReturnsTrueAndInstance()
        {
            CommandRegistry registry = new CommandRegistry();
            StubCommand command = new StubCommand("Play", "play");
            registry.Register(command);

            bool found = registry.TryGet("Play", out ICommand resolved);

            Assert.That(found, Is.True);
            Assert.That(resolved, Is.SameAs(command));
        }

        [Test]
        public void TryGet_IsCaseInsensitive()
        {
            CommandRegistry registry = new CommandRegistry();
            registry.Register(new StubCommand("Play", "play"));

            bool found = registry.TryGet("play", out ICommand _);

            Assert.That(found, Is.True);
        }

        [Test]
        public void TryGet_UnknownCommand_ReturnsFalse()
        {
            CommandRegistry registry = new CommandRegistry();

            bool found = registry.TryGet("nope", out ICommand _);

            Assert.That(found, Is.False);
        }

        [Test]
        public void Register_SameName_OverwritesPrevious()
        {
            CommandRegistry registry = new CommandRegistry();
            StubCommand first = new StubCommand("Play", "first");
            StubCommand second = new StubCommand("Play", "second");

            registry.Register(first);
            registry.Register(second);
            registry.TryGet("Play", out ICommand resolved);

            Assert.That(resolved, Is.SameAs(second));
        }

        [Test]
        public void All_ReturnsRegisteredCommands()
        {
            CommandRegistry registry = new CommandRegistry();
            registry.Register(new StubCommand("Play", "p"));
            registry.Register(new StubCommand("Exit", "e"));

            Assert.That(registry.All.Select(c => c.Name),
                Is.EquivalentTo(new[] { "Play", "Exit" })
            );
        }
    }
}
