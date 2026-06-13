using Domain.ValueObjects;

namespace Domain.Tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Constructor_ValidArguments_CreatesMoney()
    {
        // Arrange
        Money sut = new Money( 100m, "EUR" );

        // Act
        decimal amount = sut.Amount;
        string currency = sut.Currency;

        // Assert
        Assert.Equal( 100m, amount );
        Assert.Equal( "EUR", currency );
    }

    [Fact]
    public void Constructor_NegativeAmount_Throws()
    {
        // Arrange 
        string currency = "EUR";
        decimal amount = -100m;

        // Act, Assert
        Assert.Throws<ArgumentOutOfRangeException>( () => new Money( amount, currency ) );
    }

    [Fact]
    public void Constructor_EmptyCurrency_Throws()
    {
        // Arrange 
        string currency = "";
        decimal amount = 100m;

        // Act, Assert
        Assert.Throws<ArgumentException>( () => new Money( amount, currency ) );
    }

    [Fact]
    public void Constructor_WhitespaceCurrency_Throws()
    {
        // Arrange 
        string currency = " ";
        decimal amount = 100m;

        // Act, Assert
        Assert.Throws<ArgumentException>( () => new Money( amount, currency ) );
    }

    [Fact]
    public void Multiply_PositiveMultiplier_ReturnsScaledMoney()
    {
        // Arrange
        Money sut = new Money( 100m, "EUR" );

        // Act
        Money result = sut.Multiply( 3 );

        // Assert
        Assert.Equal( 300m, result.Amount );
        Assert.Equal( "EUR", result.Currency );
    }

    [Fact]
    public void Multiply_ZeroMultiplier_ReturnsZeroAmount()
    {
        // Arrange
        Money sut = new Money( 100m, "EUR" );

        // Act
        Money result = sut.Multiply( 0 );

        // Assert
        Assert.Equal( 0m, result.Amount );
    }

    [Fact]
    public void Multiply_NegativeFactor_Throws()
    {
        // Arrange
        Money sut = new Money( 100m, "EUR" );

        // Act, Assert
        Assert.Throws<ArgumentOutOfRangeException>( () => sut.Multiply( -1 ) );
    }

    [Fact]
    public void Equality_SameAmountAndCurrency_AreEqual()
    {
        // Arrange
        string currency = "EUR";
        decimal amount = 100m;

        // Act
        Money sut1 = new Money( amount, currency );
        Money sut2 = new Money( amount, currency );

        // Assert
        Assert.Equal( sut1, sut2 );
    }

    [Fact]
    public void Equality_DifferentCurrency_AreNotEqual()
    {
        // Arrange
        string currency1 = "EUR";
        string currency2 = "USD";
        decimal amount = 100m;

        // Act
        Money sut1 = new Money( amount, currency1 );
        Money sut2 = new Money( amount, currency2 );

        // Assert
        Assert.NotEqual( sut1, sut2 );
    }
}