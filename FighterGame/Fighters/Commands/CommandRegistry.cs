namespace Fighters.Commands
{
    public class CommandRegistry
    {
        private readonly Dictionary<string, ICommand> _commands;

        public CommandRegistry(IEnumerable<ICommand> commands)
        {
            ArgumentNullException.ThrowIfNull(commands);
            _commands = commands.ToDictionary(command => command.Name, StringComparer.OrdinalIgnoreCase);
        }

        public IReadOnlyCollection<ICommand> All => _commands.Values;

        public bool TryToGet(string name, out ICommand command)
        {
            return _commands.TryGetValue(name, out command!);
        }
    }
}
