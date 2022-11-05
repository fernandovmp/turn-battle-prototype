using Rpg2d.Battle.Actors;
using Rpg2d.Skills;

namespace Rpg2d.Battle.Actions
{
    public class BattleAction
    {
        public Skill Skill { get; set; }
        public ITargetGroup TargetGroup { get; set; }
        public IBattlerSlot Owner { get; set; }

        public void Reset(Skill skill) => Select(skill, null);
        public void Select(Skill skill, ITargetGroup targetGroup)
        {
            Skill = skill;
            TargetGroup = targetGroup;
        }

    }
}