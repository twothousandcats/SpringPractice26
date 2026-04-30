namespace Fighters.Commands
{
    public class ExitCommand : ICommand
    {
        public string Name => "Exit";
        public string Description => "Exits the game";

        public bool Requested { get; private set; }

        public void Execute() => Requested = true;
    }
}
