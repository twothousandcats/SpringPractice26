namespace Domain.Exceptions;

public sealed class BookingValidationException : Exception
{
    public BookingValidationException( string message ) : base( message )
    {
    }
}