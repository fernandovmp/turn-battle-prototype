using Godot;

namespace Rpg2d.Godot.Skills
{
    public class SkillAnimation
    {
        public string Name { get; set; }
        public string Animation { get; set; }
        public SpriteFrames Frames { get; set; }
        public int[] HitFrames { get; set; }
        public Vector2 CustomScale { get; set; }
    }
}