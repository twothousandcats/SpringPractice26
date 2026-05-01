using Fighters.Battle;
using Fighters.Commands;
using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters
{
    public class Program
    {
        public static void Main()
        {
            Random rng = new();
            List<IFighter> arena = [];
            IConsole console = new SystemConsole();

            IDamageCalculator damageCalc =
                new CriticalHitDamageCalculator(
                    new RandomVarianceDamageCalculator(new BaseDamageCalculator(), rng),
                    rng
                );

            GameManager gameManager = new(
                new ConsoleBattleLogger(console),
                new WeakestTargetSelector(),
                damageCalc
            );

            IFighterFactory fighterFactory = new ConsoleFighterFactory(
                console,
                FighterOptionsRegistry.Armors,
                FighterOptionsRegistry.Classes,
                FighterOptionsRegistry.Races,
                FighterOptionsRegistry.Weapons
            );

            ExitCommand quit = new();
            CommandRegistry registry = null!;
            HelpCommand help = new(() => registry.All, console);
            ICommand[] commands =
            [
                new AddFighterCommand(arena, fighterFactory, console),
                new ListFightersCommand(arena, console),
                new RemoveFighterCommand(arena, console),
                new PlayCommand(arena, gameManager, console),
                help,
                quit
            ];

            registry = new CommandRegistry(commands);
            new CommandLoop(registry, quit, console).Run();
        }
    }
}
