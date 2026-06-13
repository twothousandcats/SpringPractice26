using Domain.ValueObjects;

namespace Domain.Tests.ValueObjects;

public class DateRangeTests
{
    [Fact]
    public void Constructor_EndAfterStart_CreatesRange()
    {
        // Arrange
        DateOnly start = new DateOnly( 2026, 5, 1 );
        DateOnly end = new DateOnly( 2026, 5, 3 );
        DateRange sut = new DateRange( start, end );

        // Act
        DateOnly startDate = sut.Start;
        DateOnly endDate = sut.End;

        // Assert
        Assert.Equal( start, startDate );
        Assert.Equal( end, endDate );
    }

    [Fact]
    public void Constructor_EndEqualsStart_Throws()
    {
        // Arrange
        DateOnly date = new DateOnly( 2026, 6, 1 );

        // Act, Assert
        Assert.Throws<ArgumentException>( () => new DateRange( date, date ) );
    }

    [Fact]
    public void Constructor_EndBeforeStart_Throws()
    {
        // Arrange
        DateOnly start = new DateOnly( 2026, 5, 5 );
        DateOnly end = new DateOnly( 2026, 5, 4 );

        // Act, Assert
        Assert.Throws<ArgumentException>( () => new DateRange( start, end ) );
    }

    [Fact]
    public void Constructor_StartEqualsEnd_Throws()
    {
        // Arrange
        DateOnly start = new DateOnly( 2026, 5, 1 );
        DateOnly end = new DateOnly( 2026, 5, 10 );
        DateRange sut = new DateRange( start, end );

        // Act
        int nights = sut.Nights;

        // Assert
        Assert.Equal( 9, nights );
    }

    [Fact]
    public void Overlaps_RangesIntersect_ReturnsTrue()
    {
        // Arrange
        DateOnly start1 = new DateOnly( 2026, 5, 1 );
        DateOnly end1 = new DateOnly( 2026, 5, 10 );
        DateRange sut1 = new DateRange( start1, end1 );
        DateOnly start2 = new DateOnly( 2026, 5, 5 );
        DateOnly end2 = new DateOnly( 2026, 5, 11 );
        DateRange sut2 = new DateRange( start2, end2 );

        // Act
        bool isOverlaps = sut1.Overlaps( sut2 );

        // Assert
        Assert.True( isOverlaps );
    }

    [Fact]
    public void Overlaps_RangesTouchAtBoundary_ReturnsFalse()
    {
        // Arrange
        DateOnly start1 = new DateOnly( 2026, 5, 1 );
        DateOnly end1 = new DateOnly( 2026, 5, 10 );
        DateRange sut1 = new DateRange( start1, end1 );
        DateOnly start2 = new DateOnly( 2026, 5, 10 );
        DateOnly end2 = new DateOnly( 2026, 5, 11 );
        DateRange sut2 = new DateRange( start2, end2 );

        // Act
        bool isOverlaps = sut1.Overlaps( sut2 );

        // Assert
        Assert.False( isOverlaps );
    }

    [Fact]
    public void Overlaps_DisjointRanges_ReturnsFalse()
    {
        // Arrange
        DateOnly start1 = new DateOnly( 2026, 5, 1 );
        DateOnly end1 = new DateOnly( 2026, 5, 10 );
        DateRange sut1 = new DateRange( start1, end1 );
        DateOnly start2 = new DateOnly( 2026, 5, 15 );
        DateOnly end2 = new DateOnly( 2026, 5, 20 );
        DateRange sut2 = new DateRange( start2, end2 );

        // Act
        bool isOverlaps = sut1.Overlaps( sut2 );

        // Assert
        Assert.False( isOverlaps );
    }
}