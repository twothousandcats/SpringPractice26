using Fighters.Battle;
using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Random rng = new Random();
            IDamageCalculator damageCalculator =
                new CriticalHitDamageCalculator(
                    new RandomVarianceDamageCalculator(
                        new BaseDamageCalculator(), rng
                    ), rng
                );
            List<IFighter> arena = new List<IFighter>();
            GameManager gameManager = new GameManager(new ConsoleBattleLogger(), new WeakestTargetSelector(), new BaseDamageCalculator());
        }
    }
}
