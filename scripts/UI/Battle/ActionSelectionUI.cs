using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using TurnBattle.Godot.Battle.Actors;
using TurnBattle.Services;
using TurnBattle.UI.Controls;

namespace TurnBattle.UI.Battle
{
    public class ActionSelectionUI : Panel
    {
        private NavigableCollection<UnitSlot> _units;
        private ScrollList _skillList;
        private Label _unitName;

        public void Show(IEnumerable<UnitSlot> units)
        {
            _units = new NavigableCollection<UnitSlot>(units);
            _unitName = GetNode<Label>("Container/CurrentUnitContainer/Label");
            _skillList = GetNode<ScrollList>("Container/SkillList");
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
            _skillList.Clear();
            _skillList.AddRange(unit.Skills.Select(skill => new SkillItem(skill)));
            _skillList.FocusFirst();
        }
    }
}