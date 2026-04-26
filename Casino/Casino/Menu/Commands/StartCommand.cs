using Casino.Domain;
using Casino.Infrastructure;

namespace Casino.Menu.Commands;

public sealed class StartCommand : IMenuCommand
{
    private readonly IInputOutput _io;
    private readonly Game _game;

    public StartCommand( IInputOutput io, Game game )
    {
        ArgumentNullException.ThrowIfNull( io );
        ArgumentNullException.ThrowIfNull( game );
        _io = io;
        _game = game;
    }

    public string Title => "Начать";

    public void Execute()
    {
        _io.WriteLine( $"Игра началась. Ваш начальный баланс: {_game.Balance:0.##}" );
    }
}