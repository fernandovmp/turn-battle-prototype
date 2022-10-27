namespace Rpg2d.Battle
{
    public interface IBattlerSlot
    {
        IBattler Battler { get; }
        void PerformAction(BattleAction action);
    }
}