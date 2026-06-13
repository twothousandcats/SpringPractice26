using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Infrastructure.Repositories;

public sealed class InMemoryPropertyRepository : IPropertyRepository
{
    private readonly ConcurrentDictionary<Guid, Property> _store = new ConcurrentDictionary<Guid, Property>();

    public void Add( Property property )
    {
        if ( !_store.TryAdd( property.Id, property ) )
        {
            throw new InvalidOperationException(
                $"Property with id {property.Id} already exists"
            );
        }
    }

    public Property? Get( Guid id )
    {
        return _store.TryGetValue( id, out Property? property )
            ? property
            : null;
    }

    public IReadOnlyCollection<Property> GetAll()
    {
        return _store.Values.ToArray();
    }

    public IReadOnlyCollection<Property> GetByCity( string city )
    {
        return _store.Values
            .Where( prop => string.Equals( prop.City, city, StringComparison.OrdinalIgnoreCase ) )
            .ToArray();
    }

    public void Update( Property property )
    {
        if ( !_store.ContainsKey( property.Id ) )
        {
            throw new EntityNotFoundException(
                nameof( Property ),
                property.Id
            );
        }

        _store[ property.Id ] = property;
    }

    // soft
    public bool Remove( Guid id )
    {
        return _store.TryRemove( id, out _ );
    }
}