using System;

namespace Rpg2d.Battle
{
    public interface IBattlerSlot
    {
        IBattler Battler { get; }
        Action DamageRecived { get; set; }
        Action Died { get; set; }
        bool CanAct { get; }
        bool IsActing { get; }
        void PerformAction(BattleAction action);
    }
}