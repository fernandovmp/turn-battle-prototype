using Godot;
using Rpg2d.Battle.Actions;
using Rpg2d.Battle.Actors;
using Rpg2d.Godot.Skills;
using Rpg2d.Skills;
using System.Collections.Generic;
using System.Linq;

namespace Rpg2d.Godot.Battle.AI
{
    public class AttackAiResource : AiResource
    {
        [Export]
        private SkillResource _attackSkill;

        public override IEnumerator<BattleAction> GetActions(ActionContext context)
        {
            foreach (var entity in context.Enemies.Where(e => e.CanAct))
            {
                var units = context.Party.Where(unit => unit.Battler.Hp > 0);
                int index = (int)GD.RandRange(0, units.Count() - 1);
                IBattlerSlot target = units.ElementAt(index);

                if (target != null)
                {
                    yield return new BattleAction
                    {
                        Owner = entity,
                        Skill = _attackSkill.AsSkill(),
                        TargetGroup = new SingleTargetGroup(target)
                    };
                    yield return new WaitAction(1.5f);
                }
            }
        }
    }
}

