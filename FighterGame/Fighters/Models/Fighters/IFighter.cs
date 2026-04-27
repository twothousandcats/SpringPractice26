namespace Fighters.Models.Fighters
{
    public interface IFighter
    {
        string Name { get; }

        int GetCurrentHealth();
        int GetMaxHealth();
        int CalculateDamage();
        int CalculateArmor();

        bool IsAlive();
        void TakeDamage(int damage);
        int Initiative { get; }
    }
}
