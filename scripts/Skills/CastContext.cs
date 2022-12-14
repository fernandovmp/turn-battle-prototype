using TurnBattle.Battle.Actors;

namespace TurnBattle.Skills
{
    public class CastContext
    {
        public Skill Skill { get; set; }
        public IBattlerSlot Caster { get; set; }
        public IBattlerSlot Target { get; set; }
    }
}
