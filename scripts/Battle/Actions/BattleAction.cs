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

        public bool Execute()
        {
            if(CannotExceute())
            {
                return false;
            }
            Owner.Battler.UseMp(Skill.Cost);
            foreach (var target in TargetGroup.GetTargets())
            {
                target.HandleSkillAsTarget(new CastContext
                {
                    Caster = Owner,
                    Skill = Skill,
                    Target = target
                });
            }
            return true;
        }

        public virtual bool CanExecute() => Owner.Battler.Mp >= Skill.Cost;
        public bool CannotExceute() => !CanExecute();
    }
}
