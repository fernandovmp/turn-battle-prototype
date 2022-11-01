using Godot;

namespace Rpg2d.Battle
{
    public class BattleScene : Node
    {
        [Export]
        private UnitResource _partyLeftUnit;
        [Export]
        private UnitResource _partyUpUnit;
        [Export]
        private EnemyTroopResource _troop;

        public override void _Ready()
        {
            var battleSystem = GetNode<BattleSystem>("BattleSystem");
            battleSystem.Init(new BattleSystemContext
            {
                PartyLeftUnit = _partyLeftUnit,
                PartyUpUnit = _partyUpUnit,
                Troop = _troop
            });
        }
    }
}