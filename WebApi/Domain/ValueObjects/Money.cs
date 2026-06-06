namespace Domain.ValueObjects;

public sealed record Money
{
    public Money( decimal amount, string currency )
    {
        if ( amount < 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( amount ), "Amount cannot be negative" );
        }

        if ( string.IsNullOrWhiteSpace( currency ) )
        {
            throw new ArgumentException( "Currency is required!", nameof( currency ) );
        }

        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }

    public string Currency { get; }

    public Money Multiply( int multiplier )
    {
        if ( multiplier < 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( multiplier ), "Multiplier cannot be negative" );
        }

        return new Money( Amount * multiplier, Currency );
    }
}