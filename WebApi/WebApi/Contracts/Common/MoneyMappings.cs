using Domain.ValueObjects;

namespace WebApi.Contracts.Common;

public static class MoneyMappings
{
    public static MoneyDto ToDto( this Money money )
    {
        return new MoneyDto( money.Amount, money.Currency );
    }
}