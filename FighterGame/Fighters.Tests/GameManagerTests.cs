using Fighters.Models.Fighters;
using Fighters.Models.Races;
using NUnit.Framework;

namespace Fighters.Tests
{
    [TestFixture]
    public class GameManagerTests
    {
        [Test]
        public void Play_TwoEqualFighters_FirstFighterWins()
        {
            var gameManager = new GameManager();
            var fighterA = new Knight("FighterA", new Human());
            var fighterB = new Knight("FighterB", new Human());

            var winner = gameManager.Play(fighterA, fighterB);

            Assert.That(winner.Name, Is.EqualTo(fighterA.Name));
        }

        [Test]
        public void Play_TwoEqualFighters_SecondFighterDies()
        {
            var gameManager = new GameManager();
            var fighterA = new Knight("FighterA", new Human());
            var fighterB = new Knight("FighterB", new Human());

            gameManager.Play(fighterA, fighterB);

            Assert.That(fighterA.GetCurrentHealth(), Is.GreaterThan(0));
            Assert.That(fighterB.GetCurrentHealth(), Is.EqualTo(0));
        }
    }
}
