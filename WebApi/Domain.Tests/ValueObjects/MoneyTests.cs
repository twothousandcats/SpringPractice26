using Domain.ValueObjects;

namespace Domain.Tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Constructor_ValidArguments_CreatesMoney()
    {
        Money money = new Money( 100m, "EUR" );

        Assert.Equal( 100m, money.Amount );
        Assert.Equal( "EUR", money.Currency );
    }

    [Fact]
    public void Constructor_NegativeAmount_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>( () => new Money( -1m, "EUR" ) );
    }

    [Fact]
    public void Constructor_EmptyCurrency_Throws()
    {
        Assert.Throws<ArgumentException>( () => new Money( 100m, "" ) );
    }

    [Fact]
    public void Constructor_WhitespaceCurrency_Throws()
    {
        Assert.Throws<ArgumentException>( () => new Money( 100m, "   " ) );
    }

    [Fact]
    public void Multiply_PositiveMultiplier_ReturnsScaledMoney()
    {
        Money money = new Money( 100m, "EUR" );

        Money result = money.Multiply( 3 );

        Assert.Equal( 300m, result.Amount );
        Assert.Equal( "EUR", result.Currency );
    }

    [Fact]
    public void Multiply_ZeroMultiplier_ReturnsZeroAmount()
    {
        Money money = new Money( 100m, "EUR" );

        Money result = money.Multiply( 0 );

        Assert.Equal( 0m, result.Amount );
    }

    [Fact]
    public void Multiply_NegativeFactor_Throws()
    {
        Money money = new Money( 100m, "EUR" );

        Assert.Throws<ArgumentOutOfRangeException>( () => money.Multiply( -1 ) );
    }

    [Fact]
    public void Equality_SameAmountAndCurrency_AreEqual()
    {
        Money first = new Money( 100m, "EUR" );
        Money second = new Money( 100m, "EUR" );

        Assert.Equal( first, second );
    }

    [Fact]
    public void Equality_DifferentCurrency_AreNotEqual()
    {
        Money first = new Money( 100m, "EUR" );
        Money second = new Money( 100m, "USD" );

        Assert.NotEqual( first, second );
    }
}