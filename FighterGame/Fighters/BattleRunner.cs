using Fighters.Battle;
using Fighters.Models.Fighters;

namespace Fighters;

public class BattleRunner
{
    private const int MaxRounds = 1000;

    private readonly IBattleLogger _logger;

    private readonly ITargetSelector _targetSelector;

    private readonly IDamageCalculator _damageCalculator;

    public BattleRunner(
        IBattleLogger logger,
        ITargetSelector targetSelector,
        IDamageCalculator damageCalculator
    )
    {
        _logger = logger;
        _targetSelector = targetSelector;
        _damageCalculator = damageCalculator;
    }

    public BattleResult Play( IReadOnlyList<IFighter> fighters )
    {
        ArgumentNullException.ThrowIfNull( fighters );
        if ( fighters.Count < 2 )
        {
            throw new ArgumentException( "At least two fighters are required", nameof( fighters ) );
        }

        BattleResult battleResult = RunBattle( new List<IFighter>( fighters ) );
        _logger.LogFighterWon( battleResult.Winner! );
        return battleResult;
    }

    private BattleResult RunBattle( List<IFighter> arena )
    {
        for ( int round = 1; round <= MaxRounds; round++ )
        {
            _logger.LogAnnounceRound( round );
            int totalDamageThisRound = 0;

            IFighter[] turnOrder = arena
                .OrderByDescending( f => f.Initiative )
                .ToArray();

            foreach ( IFighter attacker in turnOrder )
            {
                if ( !attacker.IsAlive )
                {
                    continue;
                }

                IFighter? target = _targetSelector.Pick( attacker, arena );
                if ( target is null )
                {
                    _logger.LogFighterWon( attacker );
                    return BattleResult.Victory( attacker );
                }

                int dealt = ApplyAttack( attacker, target );
                totalDamageThisRound += dealt;
                _logger.LogPerformAttack( attacker, target, dealt );

                if ( !target.IsAlive )
                {
                    _logger.LogFighterDied( target );
                }
            }

            if ( totalDamageThisRound == 0 )
            {
                IFighter[] survivors = arena.Where( f => f.IsAlive ).ToArray();
                _logger.LogReachStalemate( survivors );
                return BattleResult.Stalemate( PickStrongest( survivors ) );
            }
        }

        IFighter[] remainingFighters = arena.Where( f => f.IsAlive ).ToArray();
        return BattleResult.RoundLimitReached( PickStrongest( remainingFighters ) );
    }

    private int ApplyAttack( IFighter attacker, IFighter defender )
    {
        int damage = _damageCalculator.Calculate( attacker, defender );
        defender.TakeDamage( damage );
        return damage;
    }

    private IFighter PickStrongest( IReadOnlyList<IFighter> fighters )
    {
        return fighters
                   .MaxBy( f => f.CurrentHealth ) ??
               throw new InvalidOperationException(
                   "No surviving fighters to choose a winner from."
               );
    }
}