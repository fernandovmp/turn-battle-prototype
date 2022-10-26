using Godot;
using System;

namespace Rpg2d.Skills
{
    public class SkillResource : Resource
    {
        [Export]
        public string Name { get; set; }
        [Export]
        public int Damage { get; set; }
        [Export]
        public string IdleAnimation { get; set; } = "idle";
        [Export]
        public string ActionAnimation { get; set; } = "attack";
        [Export]
        public TargetTypeEnum TargetType { get; set; }
    }
}
