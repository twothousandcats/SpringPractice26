using Fighters.Models.Fighters;

namespace Fighters.Battle
{
    public class WeakestTargetSelector : ITargetSelector
    {
        public IFighter? Pick(IFighter attacker, IReadOnlyList<IFighter> arena)
        {
            ArgumentNullException.ThrowIfNull(attacker);
            ArgumentNullException.ThrowIfNull(arena);

            IFighter? weakest = null;
            foreach (IFighter target in arena)
            {
                if (!target.IsAlive)
                {
                    continue;
                }

                if (weakest is null || target.CurrentHealth < weakest.CurrentHealth)
                {
                    weakest = target;
                }
            }

            return  weakest;
        }
    }
}
