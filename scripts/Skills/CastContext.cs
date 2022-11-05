using Rpg2d.Battle.Actors;

namespace Rpg2d.Skills
{
    public class CastContext
    {
        public Skill Skill { get; set; }
        public IBattlerSlot Caster { get; set; }
        public IBattlerSlot Target { get; set; }
    }
}
