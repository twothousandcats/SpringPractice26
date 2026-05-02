using Fighters.Battle;
using Fighters.Commands;
using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters;

public class Program
{
    public static void Main()
    {
        Random rng = new();
        List<IFighter> arena = [ ];
        IConsole console = new SystemConsole();

        IDamageCalculator damageCalc =
            new CriticalHitDamageCalculator(
                new RandomVarianceDamageCalculator( new PlainDamageCalculator(), rng ),
                rng
            );

        BattleRunner battleRunner = new(
            new ConsoleBattleLogger( console ),
            new WeakestTargetSelector(),
            damageCalc
        );

        IFighterFactory fighterFactory = new ConsoleFighterFactory(
            console,
            FighterCatalog.Armors,
            FighterCatalog.Classes,
            FighterCatalog.Races,
            FighterCatalog.Weapons
        );

        IGameLoop gameLoop = new GameLoop();
        CommandRegistry registry = new();
        registry.Register( new AddFighterConsoleCommand( arena, fighterFactory, console ) );
        registry.Register( new ListFightersConsoleCommand( arena, console ) );
        registry.Register( new RemoveFighterConsoleCommand( arena, console ) );
        registry.Register( new PlayConsoleCommand( arena, battleRunner, console ) );
        registry.Register( new HelpConsoleCommand( registry, console ) );
        registry.Register( new ExitConsoleCommand( gameLoop ) );

        new CommandLoop( registry, gameLoop, console ).Run();
    }
}