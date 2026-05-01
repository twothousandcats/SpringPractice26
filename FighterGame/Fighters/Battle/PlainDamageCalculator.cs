using Fighters.Models.Fighters;

namespace Fighters.Battle
{
    public class PlainDamageCalculator : IDamageCalculator
    {
        public int Calculate(IFighter attacker, IFighter defender)
        {
            ArgumentNullException.ThrowIfNull(attacker);
            ArgumentNullException.ThrowIfNull(defender);

            return Math.Max(attacker.Damage - defender.Armor, 0);
        }
    }
}
