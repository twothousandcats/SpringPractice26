using Fighters.Models.Fighters;

namespace Fighters.UI
{
    public interface IFighterFactory
    {
        IFighter Create();
    }
}
