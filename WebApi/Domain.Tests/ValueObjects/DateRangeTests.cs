using Domain.ValueObjects;

namespace Domain.Tests.ValueObjects;

public class DateRangeTests
{
    [Fact]
    public void Constructor_EndAfterStart_CreatesRange()
    {
        DateOnly start = new DateOnly( 2026, 5, 1 );
        DateOnly end = new DateOnly( 2026, 5, 10 );

        DateRange dateRange = new DateRange( start, end );

        Assert.Equal( start, dateRange.Start );
        Assert.Equal( end, dateRange.End );
    }

    [Fact]
    public void Constructor_EndEqualsStart_Throws()
    {
        DateOnly date = new DateOnly( 2026, 6, 1 );

        Assert.Throws<ArgumentException>( () => new DateRange( date, date ) );
    }

    [Fact]
    public void Constructor_EndBeforeStart_Throws()
    {
        DateOnly start = new DateOnly( 2026, 5, 5 );
        DateOnly end = new DateOnly( 2026, 5, 4 );

        Assert.Throws<ArgumentException>( () => new DateRange( start, end ) );
    }

    [Fact]
    public void Constructor_StartEqualsEnd_Throws()
    {
        DateOnly start = new DateOnly( 2026, 5, 1 );
        DateOnly end = new DateOnly( 2026, 5, 10 );
        DateRange dateRange = new DateRange( start, end );

        int nights = dateRange.Nights;

        Assert.Equal( 9, nights );
    }

    [Fact]
    public void Overlaps_RangesIntersect_ReturnsTrue()
    {
        DateOnly start1 = new DateOnly( 2026, 5, 1 );
        DateOnly end1 = new DateOnly( 2026, 5, 10 );
        DateRange dateRange1 = new DateRange( start1, end1 );
        DateOnly start2 = new DateOnly( 2026, 5, 5 );
        DateOnly end2 = new DateOnly( 2026, 5, 11 );
        DateRange dateRange2 = new DateRange( start2, end2 );

        bool isOverlaps = dateRange1.Overlaps( dateRange2 );

        Assert.True( isOverlaps );
    }

    [Fact]
    public void Overlaps_RangesTouchAtBoundary_ReturnsFalse()
    {
        DateOnly start1 = new DateOnly( 2026, 5, 1 );
        DateOnly end1 = new DateOnly( 2026, 5, 10 );
        DateRange dateRange1 = new DateRange( start1, end1 );
        DateOnly start2 = new DateOnly( 2026, 5, 10 );
        DateOnly end2 = new DateOnly( 2026, 5, 11 );
        DateRange dateRange2 = new DateRange( start2, end2 );

        bool isOverlaps = dateRange1.Overlaps( dateRange2 );

        Assert.False( isOverlaps );
    }

    [Fact]
    public void Overlaps_DisjointRanges_ReturnsFalse()
    {
        DateOnly start1 = new DateOnly( 2026, 5, 1 );
        DateOnly end1 = new DateOnly( 2026, 5, 10 );
        DateRange dateRange1 = new DateRange( start1, end1 );
        DateOnly start2 = new DateOnly( 2026, 5, 15 );
        DateOnly end2 = new DateOnly( 2026, 5, 20 );
        DateRange dateRange2 = new DateRange( start2, end2 );

        bool isOverlaps = dateRange1.Overlaps( dateRange2 );

        Assert.False( isOverlaps );
    }
}