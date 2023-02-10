using System;
using TurnBattle.Battle.Actors;
using TurnBattle.Skills;

namespace TurnBattle.Battle.Actions
{
    public class BattleAction
    {
        public Skill Skill { get; set; }
        public ITargetGroup TargetGroup { get; set; }
        public IBattlerSlot Owner { get; set; }
        public Action ActionChange { get; set; }

        public void Reset(Skill skill) => Select(skill, null);
        public void Select(Skill skill, ITargetGroup targetGroup)
        {
            Skill = skill;
            TargetGroup = targetGroup;
            ActionChange?.Invoke();
        }

    }
}
