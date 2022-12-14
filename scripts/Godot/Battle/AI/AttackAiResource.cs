using Godot;
using TurnBattle.Battle.Actions;
using TurnBattle.Battle.Actors;
using TurnBattle.Godot.Skills;
using TurnBattle.Skills;
using System.Collections.Generic;
using System.Linq;

namespace TurnBattle.Godot.Battle.AI
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

