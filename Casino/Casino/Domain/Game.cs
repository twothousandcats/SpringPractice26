using Casino.Infrastructure;

namespace Casino.Domain;

public sealed class Game
{
    private const int MinRoll = 1;

    private const int MaxRoll = 20;

    private const int FirstWinningNumber = 18;

    private const int PayoutDivisor = 17;

    private readonly IRandomGenerator _randomGenerator;

    private readonly int _multiplier;

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
                "Мультипликатор должен быть положительным"
            );
        }

        _balance = initialBalance;
        _multiplier = multiplicator;
        _randomGenerator = randomGenerator;
    }

    public decimal Balance => _balance;

    public RoundResult PlayRound( decimal bet )
    {
        int rolled = _randomGenerator.NextInclusive( MinRoll, MaxRoll );
        bool isWin = rolled >= FirstWinningNumber;
        decimal payout = isWin ? CalculateWinPayout( bet, rolled ) : -bet;
        _balance += payout;

        return new RoundResult( rolled, isWin, bet, payout, _balance );
    }

    private decimal CalculateWinPayout( decimal bet, int rollNumber )
    {
        // {bet} * ({multiplicator} * {random_num} % 17)
        return bet * ( _multiplier * rollNumber % PayoutDivisor );
    }
}