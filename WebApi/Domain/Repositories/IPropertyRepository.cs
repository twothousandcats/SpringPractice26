using Domain.Entities;

namespace Domain.Repositories;

public interface IPropertyRepository
{
    void Add( Property property );
    Property? Get( Guid id );
    IReadOnlyCollection<Property> List();
    IReadOnlyCollection<Property> ListByCity( string city );
    void Update( Property property );
    bool Remove( Guid id );
}