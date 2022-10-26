using Godot;
using Rpg2d.Skills;
using System;

namespace Rpg2d.Battle
{
    public class BattleAction
    {
        public Skill Skill { get; set; }
        public ITargetGroup TargetGroup { get; set; }

        public void Reset(Skill skill) => Select(skill, null);
        public void Select(Skill skill, ITargetGroup targetGroup)
        {
            Skill = skill;
            TargetGroup = targetGroup;
        }

    }
}
