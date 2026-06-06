namespace Domain.Services;

public interface ISearchService
{
    IReadOnlyCollection<SearchVariant> Search( SearchCriteria searchCriteria );
}