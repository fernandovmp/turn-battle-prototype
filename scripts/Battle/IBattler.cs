namespace Rpg2d.Battle
{
    public interface IBattler
    {
        string Name { get; }
        int MaxHp { get; }
        int Hp { get; set; }
        int Attack { get; }
    }
}
