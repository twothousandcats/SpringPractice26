using System.Globalization;
using Casino.Infrastructure;

namespace Casino.Menu.Commands;

public sealed class ConsoleBetReader : IBetReader
{
    private readonly decimal MinBet = 0;

    private IInputOutput _io;

    public ConsoleBetReader( IInputOutput io )
    {
        _io = io;
    }

    public decimal Read( decimal maxAvailableBet )
    {
        if ( maxAvailableBet <= MinBet )
        {
            throw new ArgumentException(
                nameof( maxAvailableBet ), "Доступный для ставки баланс должен быть положительным"
            );
        }

        while ( true )
        {
            _io.Write( $"Введите вашу ставку (доступно для ставки: {maxAvailableBet:0.##}): " );
            string? input = _io.ReadLine();
            if ( !TryParseBet( input, out decimal bet ) )
            {
                _io.WriteLine( "Некорректная ставка. Пожалуйста введите положительное число." );
                continue;
            }

            if ( bet > maxAvailableBet )
            {
                _io.WriteLine( "Ставка не может быть больше баланса." );
                continue;
            }

            return bet;
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
}