namespace Casino.Domain;

public readonly record struct RoundResult(
    int RolledNumber,
    bool IsWin,
    decimal Payout,
    decimal NewBalance
);