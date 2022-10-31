using Godot;
using Rpg2d.Battle;
using System.Collections.Generic;

namespace Rpg2d.UI.Battle
{
    public class BattleUi : Node
    {
        [Export]
        private PackedScene _unitHudModel;
        private Control _targetHudRoot;

        public void InitUnitHuds(IEnumerable<UnitSlot> units)
        {
            var hudRoot = GetNode<Node>("UnitHudContainer");
            foreach (var unit in units)
            {
                var hudNode = _unitHudModel.Instance();
                var hud = hudNode as UnitHud;
                hud.Initialize();
                hud.SetUnit(unit);
                hudRoot.AddChild(hud);
            }
        }

        public void UpdateTargetHud(IBattlerSlot target)
        {
            if (_targetHudRoot == null)
            {
                _targetHudRoot = GetNode<Control>("TargetHudContainer");
            }
            var hud = _targetHudRoot.GetNodeOrNull<UnitHud>("TargetHud");
            if (hud == null)
            {
                var hudNode = _unitHudModel.Instance();
                hud = hudNode as UnitHud;
                hud.Initialize();
                hud.Name = "TargetHud";
                _targetHudRoot.AddChild(hud);
            }
            _targetHudRoot.Visible = true;
            hud.SetUnit(target);
        }

        public void ShowTargetHud(bool show)
        {
            if (_targetHudRoot == null)
            {
                _targetHudRoot = GetNode<Control>("TargetHudContainer");
            }
            _targetHudRoot.Visible = show;
        }
    }
}
