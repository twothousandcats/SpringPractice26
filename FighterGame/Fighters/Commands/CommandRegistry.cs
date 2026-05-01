namespace Fighters.Commands
{
    public class CommandRegistry
    {
        private readonly Dictionary<string, ICommand> _commands = new(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyCollection<ICommand> All => _commands.Values;

        public void Register(ICommand command)
        {
            _commands[command.Name] = command;
        }

        public bool TryGet(string name, out ICommand command)
        {
            return _commands.TryGetValue(name, out command!);
        }
    }
}
