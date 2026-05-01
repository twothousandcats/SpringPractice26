using Fighters.Commands;
using Fighters.Tests.Helpers;
using NUnit.Framework;

namespace Fighters.Tests
{
    [TestFixture]
    public class HelpCommandTests
    {
        [Test]
        public void Execute_PrintsAllRegisteredCommands()
        {
            FakeConsole console = new FakeConsole();
            CommandRegistry registry = new CommandRegistry();
            registry.Register(new StubCommand("Play", "Starts the battle"));
            registry.Register(new StubCommand("Exit", "Exits the game"));
            HelpCommand help = new HelpCommand(registry, console);
            registry.Register(help);

            help.Execute();

            Assert.That(console.Output, Has.Some.Contains("Play").And.Contain("Starts"));
            Assert.That(console.Output, Has.Some.Contains("Exit").And.Contain("Exits"));
            Assert.That(console.Output, Has.Some.Contains("Help"));
        }
    }
}
