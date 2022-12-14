using TurnBattle.Godot.Battle.Actors;

namespace TurnBattle.Godot.Battle
{
    public class BattleSystemContext
    {
        public UnitResource PartyLeftUnit { get; set; }
        public UnitResource PartyUpUnit { get; set; }
        public UnitResource PartyRightUnit { get; set; }
        public UnitResource PartyBottomUnit { get; set; }
        public EnemyTroopResource Troop { get; set; }
    }
}