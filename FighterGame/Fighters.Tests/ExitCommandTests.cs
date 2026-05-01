using Fighters.Commands;
using NUnit.Framework;

namespace Fighters.Tests
{
    [TestFixture]
    public class ExitCommandTests
    {
        [Test]
        public void Execute_RequestsLifetimeStop()
        {
            ApplicationLifetime lifetime = new ApplicationLifetime();
            ExitCommand command = new ExitCommand(lifetime);

            command.Execute();

            Assert.That(lifetime.ShouldStop, Is.True);
        }
    }
}
