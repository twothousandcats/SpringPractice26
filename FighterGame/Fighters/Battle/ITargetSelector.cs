using Fighters.Models.Fighters;

namespace Fighters.Battle;

public interface ITargetSelector
{
    IFighter? Pick( IFighter attacker, IReadOnlyList<IFighter> arena );
}