namespace WebApi.Contracts.Common;

public sealed record MoneyDto(
    decimal Amount,
    string Currency
);