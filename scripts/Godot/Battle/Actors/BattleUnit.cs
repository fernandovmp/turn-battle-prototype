using System.Collections.Generic;
using System.Linq;
using TurnBattle.Battle.Stats;

namespace TurnBattle.Godot.Battle.Actors
{
    public class BattleUnit : BaseBattler
    {
        private readonly List<TurnBattle.Skills.Skill> _skills;

        public BattleUnit(UnitResource unit)
        {
            Name = unit.Name;
            Hp = new RangeStat(unit.MaxHp);
            Mp = new RangeStat(unit.MaxMp);
            Attack = unit.Attack;
            AnimationFrames = unit.AnimationFrames;
            AttackSkill = unit.AttackSkill.AsSkill();
            AttackSkill.Cost = 0;
            _skills = unit.Skills?.Select(x => x.AsSkill()).ToList() ?? new List<TurnBattle.Skills.Skill>();
            _skills.Insert(0, AttackSkill);
        }

        public IReadOnlyCollection<TurnBattle.Skills.Skill> Skills => _skills;
    }
}
