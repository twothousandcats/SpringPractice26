using Fighters.Battle;
using Fighters.Models.Fighters;

namespace Fighters;

public interface IBattleRunner
{
    BattleResult Play( IReadOnlyList<IFighter> fighters );
}