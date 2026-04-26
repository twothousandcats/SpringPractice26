using Casino.Infrastructure;

namespace Casino.Domain;

public sealed class Game
{
    private const int _minRoll = 1;
    private const int _maxRoll = 20;
    private const int _firstWinningNumber = 18;
    private const int _payoutDivisor = 17;

    private readonly IRandomGenerator _randomGenerator;
    private readonly int _multiplicator;
    private decimal _balance;

    public Game( decimal initialBalance, int multiplicator, IRandomGenerator randomGenerator )
    {
        if ( initialBalance < 0 )
        {
            throw new ArgumentOutOfRangeException(
                nameof( initialBalance ),
                "Баланс не может быть отрицательным"
            );
        }

        if ( multiplicator <= 0 )
        {
            throw new ArgumentOutOfRangeException(
                nameof( multiplicator ),
                "Мультипликатор должен быть положиетльным"
            );
        }

        ArgumentNullException.ThrowIfNull( randomGenerator );

        _balance = initialBalance;
        _multiplicator = multiplicator;
        _randomGenerator = randomGenerator;
    }

    public decimal Balance => _balance;

    public RoundResult PlayRound( decimal bet )
    {
        EnsureBetIsValid( bet );
        int rolled = _randomGenerator.NextInclusive( _minRoll, _maxRoll );
        bool isWin = rolled >= _firstWinningNumber;
        decimal payout = isWin ? CalculateWinPayout( bet, rolled ) : -bet;
        _balance += payout;

        return new RoundResult( rolled, isWin, bet, payout, _balance );
    }

    private void EnsureBetIsValid( decimal bet )
    {
        if ( bet <= 0 )
        {
            throw new ArgumentOutOfRangeException(
                nameof( bet ),
                "Ставка не может быть отрицательной"
            );
        }

        if ( bet > _balance )
        {
            throw new ArgumentOutOfRangeException(
                nameof( bet ),
                "Ставка не может быть больше баланса"
            );
        }
    }

    private decimal CalculateWinPayout( decimal bet, int rollNumber )
    {
        // bet * (1 + (multiplicator * rolledNumber) % 17)
        int remainder = ( _multiplicator * rollNumber ) % _payoutDivisor;
        return bet * ( 1 + remainder );
    }
}