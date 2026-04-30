using Fighters.UI;

namespace Fighters.Commands
{
    public class HelpCommand : ICommand
    {
        private readonly Func<IReadOnlyCollection<ICommand>> _provider;
        private readonly IConsole _console;

        public HelpCommand(Func<IReadOnlyCollection<ICommand>> provider, IConsole console)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public string Name => "Help";
        public string Description => "Show available commands";

        public void Execute()
        {
            foreach (ICommand command in _provider())
            {
                _console.WriteLine($"{command.Name}: {command.Description}");
            }
        }
    }
}
