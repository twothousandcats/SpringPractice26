using Casino.Domain;
using Casino.Infrastructure;

namespace Casino.Menu.Commands;

public sealed class ShowBalanceCommand : IMenuCommand
{
    private readonly IInputOutput _io;

    private readonly Game _game;

    public ShowBalanceCommand( IInputOutput io, Game game )
    {
        _io = io;
        _game = game;
    }

    public void Execute()
    {
        _io.WriteLine( $"Текущий баланс: {_game.Balance:0.##}" );
    }
}