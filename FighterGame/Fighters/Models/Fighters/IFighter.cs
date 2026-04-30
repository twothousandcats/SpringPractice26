namespace Fighters.Models.Fighters
{
    public interface IFighter : IName
    {
        int GetCurrentHealth();
        int GetMaxHealth();
        int CalculateDamage();
        int CalculateArmor();

        bool IsAlive();
        void TakeDamage(int damage);
        int Initiative { get; }
    }
}
