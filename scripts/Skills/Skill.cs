using Godot;
using System;

namespace Rpg2d.Skills
{
    public class Skill
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public string IdleAnimation { get; set; }
        public string ActionAnimation { get; set; }
        public TargetTypeEnum TargetType { get; set; }

        public void Cast(CastContext context)
        {
            int damage = context.Caster.Battler.Attack + Damage;
            context.Target.DealDamage(damage);
        }
    }
}
