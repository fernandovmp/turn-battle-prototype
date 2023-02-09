using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using TurnBattle.Godot.Battle.Actors;
using TurnBattle.Godot.Resources;
using TurnBattle.Services;

namespace TurnBattle.UI.Battle
{
    public class ActionSelectionUI : Panel
    {
        private NavigableCollection<UnitSlot> _units;
        private VFlowContainer _skillList;
        private Label _unitName;

        public void Show(IEnumerable<UnitSlot> units)
        {
            _units = new NavigableCollection<UnitSlot>(units);
            _unitName = GetNode<Label>("Container/CurrentUnitContainer/Label");
            _skillList = GetNode<VFlowContainer>("Container/SkillList");
            Visible = true;
            ChangeUnit(_units.Current);
        }

        public override void _Input(InputEvent inputEvent)
        {
            if(inputEvent.IsActionPressed("ui_right"))
            {
                _units.Next();
                ChangeUnit(_units.Current);
            }
            else if(inputEvent.IsActionPressed("ui_left"))
            {
                _units.Previous();
                ChangeUnit(_units.Current);
            }
            else if(inputEvent.IsActionPressed("ui_cancel"))
            {
                Hide();
            }
        }

        private void ChangeUnit(UnitSlot unitSlot)
        {
            var unit = (BattleUnit)unitSlot.Battler;
            _unitName.Text = unit.Name;
            var childs = _skillList.GetChildren();
            foreach(var child in childs)
            {
                _skillList.RemoveChild((Node)child);
            }

            foreach(var skill in unit.Skills)
            {
                var button = new Button();
                button.Text = skill.Name;
                _skillList.AddChild(button);
            }
        }
    }
}