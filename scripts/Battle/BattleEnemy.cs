using Godot;
using Rpg2d.Skills;
using System;

namespace Rpg2d.Battle
{
    public class BattleEnemy
    {
        public string Name { get; set; }
        public int MaxHp { get; set; }
        public int Hp { get; set; }
        public int Attack { get; set; }
        public SpriteFrames AnimationFrames { get; set; }
        public Skill AttackSkill { get; set; }

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
