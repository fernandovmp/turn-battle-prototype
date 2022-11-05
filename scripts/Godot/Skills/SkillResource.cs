using Godot;
using Rpg2d.Skills;

namespace Rpg2d.Godot.Skills
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

        public Skill AsSkill() => new Skill
        {
            Name = Name,
            Damage = Damage,
            IdleAnimation = IdleAnimation,
            ActionAnimation = ActionAnimation,
        };
    }
}
