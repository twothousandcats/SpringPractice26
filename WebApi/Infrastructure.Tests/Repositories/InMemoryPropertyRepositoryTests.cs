using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Repositories;

namespace Infrastructure.Tests.Repositories;

public class InMemoryPropertyRepositoryTests
{
    private const string HotelName = "Azimut";

    private const string HotelCountry = "Russia";

    private const string HotelCity = "Yoshkar-Ola";

    private const string HotelAddress = "Voskresensky Prospect, Building 11";

    private const double HotelLatitude = 56.63;

    private const double HotelLongitude = 47.91;

    private readonly InMemoryPropertyRepository _repository = new InMemoryPropertyRepository();

    private static Property CreateProperty( string city )
    {
        return new Property(
            Guid.NewGuid(),
            HotelName,
            HotelCountry,
            city,
            HotelAddress,
            HotelLatitude,
            HotelLongitude
        );
    }

    [Fact]
    public void Add_Then_Get_ReturnsSameInstance()
    {
        Property property = CreateProperty( HotelCountry );

        _repository.Add( property );
        Property? loaded = _repository.Get( property.Id );

        Assert.Same( property, loaded );
    }

    [Fact]
    public void Get_UnknownId_ReturnsNull()
    {
        Assert.Null( _repository.Get( Guid.NewGuid() ) );
    }

    [Fact]
    public void Add_Duplicate_Throws()
    {
        Property property = CreateProperty( HotelCountry );
        _repository.Add( property );

        Assert.Throws<InvalidOperationException>( () => _repository.Add( property ) );
    }

    [Fact]
    public void GetAll_ReturnsEveryAddedProperty()
    {
        Property first = CreateProperty( HotelCountry );
        Property second = CreateProperty( HotelCity );
        _repository.Add( first );
        _repository.Add( second );

        IReadOnlyCollection<Property> all = _repository.GetAll();

        Assert.Equal( 2, all.Count );
        Assert.Contains( first, all );
        Assert.Contains( second, all );
    }

    [Fact]
    public void GetByCity_FiltersByCity_CaseInsensitive()
    {
        Property first = CreateProperty( HotelCountry );
        Property second = CreateProperty( "London" );
        _repository.Add( first );
        _repository.Add( second );

        IReadOnlyCollection<Property> result = _repository.GetByCity( HotelCountry );

        Property single = Assert.Single( result );
        Assert.Same( first, single );
    }

    [Fact]
    public void Update_Existing_DoesNotThrow()
    {
        Property property = CreateProperty( HotelCountry );
        _repository.Add( property );

        _repository.Update( property );
    }

    [Fact]
    public void Update_Unknown_ThrowsEntityNotFound()
    {
        Property property = CreateProperty( HotelCountry );

        Assert.Throws<EntityNotFoundException>( () => _repository.Update( property ) );
    }

    [Fact]
    public void Remove_Existing_ReturnsTrueAndRemoves()
    {
        Property property = CreateProperty( HotelCountry );
        _repository.Add( property );

        bool removed = _repository.Remove( property.Id );

        Assert.True( removed );
        Assert.Null( _repository.Get( property.Id ) );
    }

    [Fact]
    public void Remove_Unknown_ReturnsFalse()
    {
        Assert.False( _repository.Remove( Guid.NewGuid() ) );
    }
}