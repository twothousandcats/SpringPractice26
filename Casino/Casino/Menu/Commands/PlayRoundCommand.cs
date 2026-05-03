using System.Globalization;
using Casino.Domain;
using Casino.Infrastructure;

namespace Casino.Menu.Commands;

public sealed class PlayRoundCommand : IMenuCommand
{
    private readonly IInputOutput _io;

    private readonly Game _game;

    public PlayRoundCommand( IInputOutput io, Game game )
    {
        _io = io;
        _game = game;
    }

    public string Title => "Сыграть раунд";

    public void Execute()
    {
        if ( _game.Balance <= 0 )
        {
            _io.WriteLine( "Ваш баланс пуст. Вы не можете сделать ставку." );
            return;
        }


        decimal bet = ReadBet();
        if ( bet > _game.Balance )
        {
            _io.WriteLine( "Ставка не может быть больше баланса." );
            return;
        }

        try
        {
            RoundResult result = _game.PlayRound( bet );
            ReportResult( result );
        }
        catch ( ArgumentException ex )
        {
            _io.WriteLine( $"Не удалось сыграть: {ex.Message}" );
        }
    }

    private decimal ReadBet()
    {
        while ( true )
        {
            _io.Write( $"Введите вашу ставку (доступно для ставки: {_game.Balance:0.##}): " );
            string? input = _io.ReadLine();
            if ( TryParseBet( input, out decimal bet ) )
            {
                return bet;
            }

            _io.WriteLine( "Некорректная ставка. Пожалуйста введите положительное число." );
        }
    }

    private static bool TryParseBet( string? input, out decimal bet )
    {
        bet = 0m;
        if ( string.IsNullOrWhiteSpace( input ) )
        {
            return false;
        }

        if ( !decimal.TryParse( input, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal parsed ) )
        {
            return false;
        }

        if ( parsed <= 0 )
        {
            return false;
        }

        bet = parsed;
        return true;
    }

    private void ReportResult( RoundResult result )
    {
        _io.WriteLine( $"Выпал номер: {result.RolledNumber}" );
        _io.WriteLine(
            result.IsWin
                ? $"Вы выиграли: {result.Payout:0.##}!"
                : $"Вы проиграли вашу ставку {result.BetAmount:0.##}!"
        );

        _io.WriteLine( $"Ваш баланс: {result.NewBalance:0.##}" );
    }
}