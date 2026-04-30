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
                new ConsoleBattleLogger(),
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
            ICommand[] commands =
            [
                new AddFighterCommand(arena, fighterFactory, console),
                new PlayCommand(arena, gameManager, console),
                quit,
            ];

            CommandRegistry registry = new(commands);
            new CommandLoop(registry, quit, console).Run();
        }
    }
}
