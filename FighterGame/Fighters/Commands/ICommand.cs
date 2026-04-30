namespace Fighters.Commands
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        void Execute();
    }
}
