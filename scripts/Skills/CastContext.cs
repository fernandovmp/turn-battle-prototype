using Godot;
using Rpg2d.Battle;
using System;

namespace Rpg2d.Skills
{
    public class CastContext
    {
        public Skill Skill { get; set; }
        public IBattler Caster { get; set; }
        public IBattler Target { get; set; }
    }
}
