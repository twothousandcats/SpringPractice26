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
        FighterRoster fighterRoster = new();
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
        registry.Register( new AddFighterConsoleCommand( fighterRoster, fighterFactory, console ) );
        registry.Register( new ListFightersConsoleCommand( fighterRoster, console ) );
        registry.Register( new RemoveFighterConsoleCommand( fighterRoster, console ) );
        registry.Register( new PlayConsoleCommand( fighterRoster, battleRunner, console ) );
        registry.Register( new HelpConsoleCommand( registry, console ) );
        registry.Register( new ExitConsoleCommand( gameLoop ) );

        new CommandLoop( registry, gameLoop, console ).Run();
    }
}