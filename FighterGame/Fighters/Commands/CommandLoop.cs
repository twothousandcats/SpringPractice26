using Fighters.UI;

namespace Fighters.Commands
{
    public class CommandLoop
    {
        private readonly CommandRegistry _registry;
        private readonly IApplicationLifetime _appLifetime;
        private readonly IConsole _console;

        public CommandLoop(CommandRegistry registry, IApplicationLifetime appLifetime, IConsole console)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _appLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public void Run()
        {
            while (!_appLifetime.ShouldStop)
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

                if (!_registry.TryGet(input, out var command))
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
