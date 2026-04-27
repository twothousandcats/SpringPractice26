namespace Fighters.Models.Classes
{
    public class Mercenary : IFighterClass
    {
        public string Name => "Mercenary";
        public  int Damage => 8;
        public int Health => 30;
    }
}
