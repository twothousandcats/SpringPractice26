using System.Globalization;
using Casino.Domain;
using Casino.Infrastructure;

namespace Casino.Menu.Commands;

public sealed class PlayRoundCommand : IMenuCommand
{
    private readonly IInputOutput _io;

    private readonly Game _game;

    private readonly IBetReader _betReader;

    private readonly IRoundResultPrinter _roundResultPrinter;

    public PlayRoundCommand(
        IInputOutput io,
        Game game,
        IBetReader betReader,
        IRoundResultPrinter roundResultPrinter
    )
    {
        _io = io;
        _game = game;
        _betReader = betReader;
        _roundResultPrinter = roundResultPrinter;
    }

    public void Execute()
    {
        if ( _game.Balance <= 0 )
        {
            _io.WriteLine( "Ваш баланс пуст. Вы не можете сделать ставку." );
            return;
        }

        decimal bet = _betReader.Read( _game.Balance );
        RoundResult result = _game.PlayRound( bet );
        _roundResultPrinter.Print( result );
    }
}