using Domain.Entities;

namespace Domain.Repositories;

public interface IPropertyRepository
{
    void Add( Property property );
    Property? Get( Guid id );
    IReadOnlyCollection<Property> GetAll();
    IReadOnlyCollection<Property> GetByCity( string city );
    void Update( Property property );
    bool Remove( Guid id );
}