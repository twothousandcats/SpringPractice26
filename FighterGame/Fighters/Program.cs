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
                    new RandomVarianceDamageCalculator(new PlainDamageCalculator(), rng),
                    rng
                );

            BattleRunner battleRunner = new(
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

            IApplicationLifetime lifetime = new ApplicationLifetime();
            CommandRegistry registry = new();
            registry.Register(new AddFighterCommand(arena, fighterFactory, console));
            registry.Register(new ListFightersCommand(arena, console));
            registry.Register(new RemoveFighterCommand(arena, console));
            registry.Register(new PlayCommand(arena, battleRunner, console));
            registry.Register(new HelpCommand(registry, console));
            registry.Register(new ExitCommand(lifetime));

            new CommandLoop(registry, lifetime, console).Run();
        }
    }
}
