using Fighters.UI;

namespace Fighters.Commands
{
    public class CommandLoop
    {
        private readonly CommandRegistry _registry;
        private readonly ExitCommand _exit;
        private readonly IConsole _console;

        public CommandLoop(CommandRegistry registry, ExitCommand exit, IConsole console)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _exit = exit ?? throw new ArgumentNullException(nameof(exit));
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public void Run()
        {
            while (!_exit.Requested)
            {
                _console.WriteLine("");
                _console.WriteLine("Enter command");
                _console.Write("> ");
                string? input = _console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    _console.WriteLine("Unknown command");
                    continue;
                }

                if (!_registry.TryToGet(input, out var command))
                {
                    _console.WriteLine("Unknown command");
                    continue;
                }

                try
                {
                    command.Execute();
                }
                catch (ArgumentException ex)
                {
                    _console.WriteLine($"Bad argument: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    _console.WriteLine($"Operation failed: {ex.Message}");
                }
                catch (FormatException ex)
                {
                    _console.WriteLine($"Bad input: {ex.Message}");
                }
            }
        }
    }
}
