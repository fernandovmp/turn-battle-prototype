using System;
using TurnBattle.Battle.Actions;
using TurnBattle.Skills;

namespace TurnBattle.Battle.Actors
{
    public interface IBattlerSlot
    {
        IBattler Battler { get; }
        Action<SlotDamageRecivedArgs> DamageRecived { get; set; }
        Action Died { get; set; }
        bool CanAct { get; }
        bool IsActing { get; }
        bool ActionEnabled { get; set; }
        bool IsDead { get; set; }
        void PerformAction(BattleAction action);
        void DealDamage(int damage);
        void HandleSkillAsTarget(CastContext castContext);
    }
}