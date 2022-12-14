using Godot;
using TurnBattle.Skills;

namespace TurnBattle.Godot.Skills
{
    public class SkillResource : Resource
    {
        [Export]
        public string Name { get; set; }
        [Export]
        public float Multiplier { get; set; }
        [Export]
        public string IdleAnimation { get; set; } = "idle";
        [Export]
        public string ActionAnimation { get; set; } = "attack";
        [Export]
        public TargetTypeEnum TargetType { get; set; }
        [Export]
        public SkillAnimationResource Animation { get; set; }

        public Skill AsSkill() => new Skill
        {
            Name = Name,
            Multiplier = Multiplier,
            IdleAnimation = IdleAnimation,
            ActionAnimation = ActionAnimation,
            Animation = Animation.AsSkillAnimation()
        };
    }
}
