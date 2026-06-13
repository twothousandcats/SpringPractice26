using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Repositories;

namespace Infrastructure.Tests.Repositories;

public class InMemoryPropertyRepositoryTests
{
    [Fact]
    public void Add_Then_Get_ReturnsSameInstance()
    {
        // Arrange
        InMemoryPropertyRepository sut = new InMemoryPropertyRepository();
        Property property = new Property(
            Guid.NewGuid(),
            "Test name",
            "Test country",
            "Test city",
            "Test address",
            10.10,
            20.20
        );

        // Act
        sut.Add( property );
        Property? loaded = sut.Get( property.Id );

        // Assert
        Assert.Same( property, loaded );
    }

    [Fact]
    public void Get_UnknownId_ReturnsNull()
    {
        // Arrange
        InMemoryPropertyRepository sut = new InMemoryPropertyRepository();

        // Act, Assert
        Assert.Null( sut.Get( Guid.NewGuid() ) );
    }

    [Fact]
    public void Add_Duplicate_Throws()
    {
        // Arrange
        InMemoryPropertyRepository sut = new InMemoryPropertyRepository();
        Property property = new Property(
            Guid.NewGuid(),
            "Test name",
            "Test country",
            "Test city",
            "Test address",
            10.10,
            20.20
        );

        // Act
        sut.Add( property );

        // Assert
        Assert.Throws<InvalidOperationException>( () => sut.Add( property ) );
    }

    [Fact]
    public void GetAll_ReturnsEveryAddedProperty()
    {
        // Arrange
        InMemoryPropertyRepository sut = new InMemoryPropertyRepository();
        Property firstProperty = new Property(
            Guid.NewGuid(),
            "Test name",
            "Test country",
            "Test city",
            "Test address",
            10.10,
            20.20
        );

        Property secondProperty = new Property(
            Guid.NewGuid(),
            "Test name2",
            "Test country2",
            "Test city2",
            "Test address2",
            10.10,
            20.20
        );

        sut.Add( firstProperty );
        sut.Add( secondProperty );

        // Act
        IReadOnlyCollection<Property> all = sut.GetAll();

        // Assert
        Assert.Equal( 2, all.Count );
        Assert.Contains( firstProperty, all );
        Assert.Contains( secondProperty, all );
    }

    [Fact]
    public void GetByCity_FiltersByCity_CaseInsensitive()
    {
        // Arrange
        InMemoryPropertyRepository sut = new InMemoryPropertyRepository();
        Property firstProperty = new Property(
            Guid.NewGuid(),
            "Test name",
            "Test country",
            "Test city",
            "Test address",
            10.10,
            20.20
        );

        Property secondProperty = new Property(
            Guid.NewGuid(),
            "Test name2",
            "Test country2",
            "Test city2",
            "Test address2",
            10.10,
            20.20
        );
        sut.Add( firstProperty );
        sut.Add( secondProperty );

        // Act
        IReadOnlyCollection<Property> result = sut.GetByCity( "Test city" );

        // Assert
        Property single = Assert.Single( result );
        Assert.Same( firstProperty, single );
    }

    [Fact]
    public void Update_Unknown_ThrowsEntityNotFound()
    {
        // Arrange
        InMemoryPropertyRepository sut = new InMemoryPropertyRepository();
        Property property = new Property(
            Guid.NewGuid(),
            "Test name",
            "Test country",
            "Test city",
            "Test address",
            10.10,
            20.20
        );

        // Act, Assert
        Assert.Throws<EntityNotFoundException>( () => sut.Update( property ) );
    }

    [Fact]
    public void Remove_Existing_ReturnsTrueAndRemoves()
    {
        // Arrange
        InMemoryPropertyRepository sut = new InMemoryPropertyRepository();
        Property property = new Property(
            Guid.NewGuid(),
            "Test name",
            "Test country",
            "Test city",
            "Test address",
            10.10,
            20.20
        );

        sut.Add( property );

        // Act
        bool removed = sut.Remove( property.Id );

        // Assert
        Assert.True( removed );
        Assert.Null( sut.Get( property.Id ) );
    }

    [Fact]
    public void Remove_Unknown_ReturnsFalse()
    {
        // Arrange
        InMemoryPropertyRepository sut = new InMemoryPropertyRepository();

        // Act
        bool result = sut.Remove( Guid.NewGuid() );

        // Assert
        Assert.False( result );
    }
}