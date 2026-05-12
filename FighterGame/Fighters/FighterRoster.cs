using System.Collections;
using Fighters.Models.Fighters;

namespace Fighters;

public class FighterRoster : IReadOnlyList<IFighter>
{
    private readonly List<IFighter> _fighters = [ ];

    public int Count => _fighters.Count;

    public IFighter this[ int index ] => _fighters[ index ];

    public void Add( IFighter fighter )
    {
        ArgumentNullException.ThrowIfNull( fighter );
        _fighters.Add( fighter );
    }

    public void RemoveAt( int index )
    {
        _fighters.RemoveAt( index );
    }

    public void Clear()
    {
        _fighters.Clear();
    }
    
    public IEnumerator<IFighter> GetEnumerator()
    {
        return _fighters.GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}