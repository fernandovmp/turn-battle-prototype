using System.Collections.Generic;
using System.Linq;

namespace TurnBattle.Godot.Battle.Actors
{
    public class BattleUnit : BaseBattler
    {
        private readonly List<TurnBattle.Skills.Skill> _skills;

        public BattleUnit(UnitResource unit)
        {
            Name = unit.Name;
            MaxHp = unit.MaxHp;
            Hp = unit.MaxHp;
            Attack = unit.Attack;
            AnimationFrames = unit.AnimationFrames;
            AttackSkill = unit.AttackSkill.AsSkill();
            _skills = unit.Skills?.Select(x => x.AsSkill()).ToList() ?? new List<TurnBattle.Skills.Skill>();
        }

        public IReadOnlyCollection<TurnBattle.Skills.Skill> Skills => _skills;
    }
}
