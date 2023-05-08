using System;
using TurnBattle.Godot.Skills;

namespace TurnBattle.Skills
{
    public class Skill
    {
        public string Name { get; set; }
        public float Multiplier { get; set; }
        public string IdleAnimation { get; set; }
        public string ActionAnimation { get; set; }
        public TargetTypeEnum TargetType { get; set; }
        public SkillAnimation Animation { get; set; }
        public int Cost { get; set; }

        public void Cast(CastContext context)
        {
            int damage = (int)Math.Ceiling((context.Caster.Battler.Attack * Multiplier) / Animation.HitFrames.Length);
            context.Target.DealDamage(damage);
        }
    }
}
