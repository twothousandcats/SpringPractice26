namespace Fighters.Commands
{
    public class ExitCommand : ICommand
    {
        private readonly IApplicationLifetime _appLifetime;

        public ExitCommand(IApplicationLifetime applicationLifetime)
        {
            _appLifetime = applicationLifetime;
        }

        public string Name => "Exit";
        public string Description => "Exits the game";

        public void Execute() => _appLifetime.RequestStop();
    }
}
