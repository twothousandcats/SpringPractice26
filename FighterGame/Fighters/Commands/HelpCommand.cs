using Fighters.UI;

namespace Fighters.Commands
{
    public class HelpCommand : ICommand
    {
        private readonly CommandRegistry _registry;
        private readonly IConsole _console;

        public HelpCommand(CommandRegistry registry, IConsole console)
        {
            _registry = registry;
            _console = console;
        }

        public string Name => "Help";
        public string Description => "Show available commands";

        public void Execute()
        {
            foreach (ICommand command in _registry.All)
            {
                _console.WriteLine($"{command.Name}: {command.Description}");
            }
        }
    }
}
