using Fighters.Models.Fighters;

namespace Fighters.Battle
{
    public interface IDamageCalculator
    {
        int Calculate(IFighter attacker, IFighter defender);
    }
}
