using Fighters.Models.Fighters;

namespace Fighters.Battle;

public class WeakestTargetSelector : ITargetSelector
{
    public IFighter? Pick( IFighter attacker, IReadOnlyList<IFighter> arena )
    {
        return arena
            .Where( defender => defender.IsAlive && !ReferenceEquals( defender, attacker ) )
            .MinBy( defender => defender.CurrentHealth );
    }
}