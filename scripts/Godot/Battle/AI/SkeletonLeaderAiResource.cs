using Godot;
using Rpg2d.Battle.Actions;
using Rpg2d.Battle.Actors;
using Rpg2d.Godot.Skills;
using Rpg2d.Skills;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rpg2d.Godot.Battle.AI
{
    public class SkeletonLeaderAiResource : AiResource
    {
        [Export]
        private SkillResource _nomalAtack;
        [Export]
        private SkillResource _multihitAtack;

        private IBattlerSlot _skeletonLeader;
        private IEnumerable<IBattlerSlot> _skeletons;
        private Skill _normalAttackSkill;
        private Skill _multihitAtackSkill;
        private int _turn;
        private const string SkeletonLeaderName = "Skeleton Leader";

        public override void Init(ActionContext context)
        {
            _skeletonLeader = context.Enemies.First(x => x.Battler.Name == SkeletonLeaderName);
            _skeletons = context.Enemies.Where(x => x.Battler.Name != SkeletonLeaderName);
            _normalAttackSkill = _nomalAtack.AsSkill();
            _multihitAtackSkill = _multihitAtack.AsSkill();
            _turn = 0;
        }

        public override IEnumerator<BattleAction> GetActions(ActionContext context)
        {
            _turn++;
            var units = context.Party.Where(unit => unit.Battler.Hp > 0);
            int index = (int)GD.RandRange(0, units.Count() - 1);
            IBattlerSlot target = units.ElementAt(index);
            if(_turn % 3 == 0)
            {
                foreach (var entity in _skeletons.Where(e => e.CanAct))
                {
                    if (target != null)
                    {
                        yield return MultihitAttack(entity, units);
                        yield return new WaitAction(0.2f);
                    }
                }
                if(_skeletonLeader.CanAct)
                {
                    yield return new WaitAction(1f);
                    yield return NormalAttack(_skeletonLeader, target);
                }
            }
            else
            {
                foreach (var entity in _skeletons.Where(e => e.CanAct))
                {
                    if (target != null)
                    {
                        yield return NormalAttack(entity, target);
                        yield return new WaitAction(0.4f);
                    }
                }
                if(_skeletonLeader.CanAct)
                {
                    index = (int)GD.RandRange(0, units.Count() - 1);
                    target = units.ElementAt(index);
                    yield return new WaitAction(1f);
                    yield return NormalAttack(_skeletonLeader, target);
                }
            }
        }

        private BattleAction MultihitAttack(IBattlerSlot owner, IEnumerable<IBattlerSlot> units) => new BattleAction
        {
            Owner = owner,
            Skill = _multihitAtackSkill,
            TargetGroup = new MultiTargetGroup(units)
        };

        private BattleAction NormalAttack(IBattlerSlot owner, IBattlerSlot target) => new BattleAction
        {
            Owner = owner,
            Skill = _normalAttackSkill,
            TargetGroup = new SingleTargetGroup(target)
        };
    }
}

