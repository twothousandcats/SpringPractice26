using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters.Commands
{
    public class PlayCommand : ICommand
    {
        private readonly List<IFighter> _arena;
        private readonly BattleRunner _battleRunner;
        private readonly IConsole _console;

        public PlayCommand(List<IFighter> arena, BattleRunner battleRunner, IConsole console)
        {
            _arena = arena;
            _battleRunner = battleRunner;
            _console = console;
        }

        public string Name => "Play";
        public string Description => "Starts the battle";

        public void Execute()
        {
            if (_arena.Count < 2)
            {
                _console.WriteLine("You must add at least 2 fighters to play");
                return;
            }

            _battleRunner.Play(_arena);
            _arena.Clear();
        }
    }
}
