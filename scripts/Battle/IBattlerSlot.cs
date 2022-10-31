using System;

namespace Rpg2d.Battle
{
    public interface IBattlerSlot
    {
        IBattler Battler { get; }
        Action DamageRecived { get; set; }
        Action Died { get; set; }
        void PerformAction(BattleAction action);
    }
}