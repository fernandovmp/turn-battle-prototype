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

        public Skill(SkillResource skillResource)
        {
            Name = skillResource.Name;
            Damage = skillResource.Damage;
            IdleAnimation = skillResource.IdleAnimation;
            ActionAnimation = skillResource.ActionAnimation;
        }

        public void Cast(CastContext context)
        {
            context.Target.Hp -= context.Target.Attack + Damage;
            GD.Print($"{context.Target.Name} - HP: {context.Target.Hp}");
        }
    }
}
