using NUnit.Framework;

namespace Fighters.Tests
{
    [TestFixture]
    public class ApplicationLifetimeTests
    {
        [Test]
        public void NewLifetime_ShouldStop_IsFalse()
        {
            ApplicationLifetime lifetime = new ApplicationLifetime();

            bool shouldStop = lifetime.ShouldStop;

            Assert.That(shouldStop, Is.False);
        }

        [Test]
        public void RequestStop_SetsShouldStop()
        {
            ApplicationLifetime lifetime = new ApplicationLifetime();

            lifetime.RequestStop();
            bool shouldStop = lifetime.ShouldStop;

            Assert.That(shouldStop, Is.True);
        }

        [Test]
        public void RequestStop_IsIdempotent()
        {
            ApplicationLifetime lifetime = new ApplicationLifetime();

            lifetime.RequestStop();
            lifetime.RequestStop();
            bool shouldStop = lifetime.ShouldStop;

            Assert.That(shouldStop, Is.True);
        }
    }
}
