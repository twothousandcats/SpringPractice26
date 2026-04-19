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
        // validation
        if (initialBalance < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(initialBalance),
                "Initial balance cannot be negative"
            );
        }

        if (multiplicator <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(multiplicator), "Multiplicator must be positive"
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

        int rolled = _randomGenerator.NextInclusive(MinRoll, MaxRoll);
        bool isWin = rolled >= FirstWinningNumber;
        decimal payout;
        if (isWin)
        {
            payout = CalculateWinPayout(bet, rolled);
            _balance += payout;
        }
        else
        {
            payout = -bet;
            _balance -= payout;
        }

        return new RoundResult(rolled, isWin, bet, payout, _balance);
    }

    private void EnsureBetIsValid(decimal bet)
    {
        if (bet <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(bet), "Bet cannot be negative");
        }

        if (bet > _balance)
        {
            throw new ArgumentOutOfRangeException(nameof(bet), "Bet cannot be greater than balance");
        }
    }

    private decimal CalculateWinPayout(decimal bet, int rollNumber)
    {
        // bet * (1 + (multiplicator * rolledNumber) % 17)
        int remainder = (_multiplicator * rollNumber) % 17;
        return bet * (1 + remainder);
    }
}