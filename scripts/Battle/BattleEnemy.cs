using Godot;
using Rpg2d.Skills;
using System;

namespace Rpg2d.Battle
{
    public class BattleEnemy : BaseBattler
    {
        public BattleEnemy(EnemyResource unit)
        {
            Name = unit.Name;
            MaxHp = unit.MaxHp;
            Hp = unit.MaxHp;
            Attack = unit.Attack;
            AnimationFrames = unit.AnimationFrames;
            AttackSkill = new Skill(unit.AttackSkill);
        }
    }
}
