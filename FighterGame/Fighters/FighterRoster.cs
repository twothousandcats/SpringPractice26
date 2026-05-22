using Fighters.Models.Fighters;

namespace Fighters;

public sealed class FighterRoster
{
    private readonly List<IFighter> _fighters = [ ];

    public IReadOnlyList<IFighter> Fighters => _fighters;

    public void Add( IFighter fighter ) =>
        _fighters.Add( fighter ?? throw new ArgumentNullException( nameof( fighter ) ) );

    public void RemoveAt( int index ) => _fighters.RemoveAt( index );

    public void Clear() => _fighters.Clear();
}