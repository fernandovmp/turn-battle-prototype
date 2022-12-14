using Godot;
using TurnBattle.Godot.Skills;

namespace TurnBattle.Godot.Battle.Actors
{
    public class UnitResource : Resource
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
