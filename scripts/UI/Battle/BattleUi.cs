using Godot;
using Rpg2d.Battle;
using Rpg2d.Skills;
using System;
using System.Collections.Generic;

namespace Rpg2d.UI.Battle
{
    public class BattleUi : Node
    {
        [Export]
        private PackedScene _unitHudModel;

        public void InitUnitHuds(IEnumerable<UnitSlot> units)
        {
            foreach (var unit in units)
            {
                var hudNode = _unitHudModel.Instance();
                var hud = hudNode as UnitHud;
                hud.Initialize();
                hud.SetUnit(unit);
                AddChild(hud);
            }
        }
    }
}
