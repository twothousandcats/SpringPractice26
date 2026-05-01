using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters.Commands
{
    public class AddFighterCommand : ICommand
    {
        private readonly List<IFighter> _arena;
        private readonly IFighterFactory _factory;
        private readonly IConsole _console;

        public AddFighterCommand(List<IFighter> arena, IFighterFactory factory, IConsole console)
        {
            _arena = arena;
            _factory = factory;
            _console = console;
        }

        public string Name => "Add";
        public string Description => "Adds a fighter to the arena";

        public void Execute()
        {
            IFighter fighter = _factory.Create();
            _arena.Add(fighter);
            _console.WriteLine($"Fighter {fighter.Name} added to the arena");
        }
    }
}
