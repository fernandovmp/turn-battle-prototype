using Godot;

namespace TurnBattle.Godot.Skills
{
    public class SkillAnimation
    {
        public string Name { get; set; }
        public string Animation { get; set; }
        public SpriteFrames Frames { get; set; }
        public int[] HitFrames { get; set; }
        public Vector2 CustomScale { get; set; }
        public AudioStream HitEffect { get; set; }
    }
}