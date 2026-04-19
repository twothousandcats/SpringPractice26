using Casino.Infrastructure;

namespace Casino.Domain;

public sealed class Game
{
    private const int MinRoll = 1;
    private const int MaxRoll = 20;
    private const int FirstWinningNumber = 18;
    private const int PayoutDivisor = 17;

    private readonly IRandomGenerator _randomGenerator;
    private readonly int _multiplicator;
    private decimal _balance;

    public Game(decimal initialBalance, int multiplicator, IRandomGenerator randomGenerator)
    {
        if (initialBalance < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(initialBalance),
                "Баланс не может быть отрицательным"
            );
        }

        if (multiplicator <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(multiplicator),
                "Мультипликатор должен быть положиетльным"
            );
        }

        ArgumentNullException.ThrowIfNull(randomGenerator);

        _balance = initialBalance;
        _multiplicator = multiplicator;
        _randomGenerator = randomGenerator;
    }

    public decimal Balance => _balance;

    public RoundResult PlayRound(decimal bet)
    {
        EnsureBetIsValid(bet);
        var rolled = _randomGenerator.NextInclusive(MinRoll, MaxRoll);
        var isWin = rolled >= FirstWinningNumber;
        var payout = isWin ? CalculateWinPayout(bet, rolled) : -bet;
        _balance += payout;

        return new RoundResult(rolled, isWin, bet, payout, _balance);
    }

    private void EnsureBetIsValid(decimal bet)
    {
        if (bet <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(bet),
                "Ставка не может быть отрицательной"
            );
        }

        if (bet > _balance)
        {
            throw new ArgumentOutOfRangeException(
                nameof(bet),
                "Ставка не может быть больше баланса"
            );
        }
    }

    private decimal CalculateWinPayout(decimal bet, int rollNumber)
    {
        // bet * (1 + (multiplicator * rolledNumber) % 17)
        var remainder = (_multiplicator * rollNumber) % PayoutDivisor;
        return bet * (1 + remainder);
    }
}