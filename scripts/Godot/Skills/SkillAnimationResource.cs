using Godot;
using Rpg2d.Skills;

namespace Rpg2d.Godot.Skills
{
    public class SkillAnimationResource : Resource
    {
        [Export]
        public string Name { get; set; }
        [Export]
        public string Animation { get; set; } = "default";
        [Export]
        public SpriteFrames Frames { get; set; }
        [Export]
        public int[] HitFrames { get; set; }
        [Export]
        public Vector2 CustomScale { get; set; } = Vector2.One;

        public SkillAnimation AsSkillAnimation() => new SkillAnimation
        {
            Name = Name,
            Animation = Animation,
            Frames = Frames,
            HitFrames = HitFrames,
            CustomScale = CustomScale
        };
    }
}
