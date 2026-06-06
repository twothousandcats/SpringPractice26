namespace Domain.ValueObjects;

public sealed class DateRange
{
    public DateRange( DateOnly start, DateOnly end )
    {
        if ( end <= start )
        {
            throw new ArgumentException(
                "End must be after start",
                nameof( end )
            );
        }

        Start = start;
        End = end;
    }

    public DateOnly Start { get; }

    public DateOnly End { get; }

    public int Nights => End.DayNumber - Start.DayNumber;

    public bool Overlaps( DateRange range )
    {
        return Start < range.End && range.Start < End;
    }
}