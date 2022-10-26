using Godot;
using Rpg2d.Skills;
using System;

namespace Rpg2d.Battle
{
    public class EnemyResource : Resource
    {
        [Export]
        public string Name { get; set; }
        [Export]
        public int MaxHp { get; set; }
        [Export]
        public int Attack { get; set; }
        [Export]
        public SpriteFrames AnimationFrames { get; set; }
        [Export]
        public SkillResource AttackSkill { get; set; }
    }
}
