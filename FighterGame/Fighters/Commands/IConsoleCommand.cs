namespace Fighters.Commands;

public interface IConsoleCommand
{
    string Name { get; }

    string Description { get; }

    void Execute();
}