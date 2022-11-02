using Godot;
using Rpg2d.Skills;
using System.Collections.Generic;
using System.Linq;

namespace Rpg2d.Battle
{
    public class AttackAiResource : AiResource
    {
        [Export]
        private SkillResource _attackSkill;

        public override IEnumerator<BattleAction> GetActions(BattleContext context)
        {
            foreach (var entity in context.Enemies.Where(e => e.CanAct))
            {
                var units = context.Party.Where(unit => unit.Battler.Hp > 0);
                int index = (int)Godot.GD.RandRange(0, units.Count() - 1);
                IBattlerSlot target = units.ElementAt(index);

                if (target != null)
                {
                    yield return new BattleAction
                    {
                        Owner = entity,
                        Skill = new Skill(_attackSkill),
                        TargetGroup = new SingleTargetGroup(target)
                    };
                    yield return new WaitAction(1.5f);
                }
            }
        }
    }
}

