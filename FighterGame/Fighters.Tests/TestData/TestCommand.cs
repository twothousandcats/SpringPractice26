using Fighters.Commands;

namespace Fighters.Tests.TestData;

public sealed class TestCommand : IConsoleCommand
{
    public string Name { get; init; } = "Test";

    public string Description { get; init; } = "test command";

    public int ExecuteCallCount { get; private set; }

    public void Execute() => ExecuteCallCount++;
}