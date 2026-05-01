using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters.Commands
{
    public class ListFightersCommand : ICommand
    {
        private readonly List<IFighter> _arena;
        private readonly IConsole _console;

        public ListFightersCommand(List<IFighter> arena, IConsole console)
        {
            _arena = arena;
            _console = console;
        }

        public string Name => "List";
        public string Description => "Lists all fighters";

        public void Execute()
        {
            if (_arena.Count == 0)
            {
                _console.WriteLine("Arena is empty!");
                return;
            }

            for (int i = 0; i < _arena.Count; i++)
            {
                _console.WriteLine($"{i + 1}. {_arena[i].Description}");
            }
        }
    }
}
