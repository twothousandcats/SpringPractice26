using Fighters.Models.Fighters;
using Fighters.UI;

namespace Fighters.Commands
{
    public class PlayCommand : ICommand
    {
        private readonly List<IFighter> _arena;
        private readonly GameManager _gameManager;
        private readonly IConsole _console;

        public PlayCommand(List<IFighter> arena, GameManager gameManager, IConsole console)
        {
            _arena = arena ?? throw new ArgumentNullException(nameof(arena));
            _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public string Name => "Play";
        public string Description => "Starts the battle";

        public void Execute()
        {
            if (_arena.Count < 2)
            {
                _console.WriteLine("You must add at least 2 arena to play");
                return;
            }

            _gameManager.Play(_arena);
            _arena.Clear();
        }
    }
}
