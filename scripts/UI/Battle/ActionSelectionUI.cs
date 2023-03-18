using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using TurnBattle.Godot.Battle;
using TurnBattle.Godot.Battle.Actors;
using TurnBattle.Godot.Resources;
using TurnBattle.Services;
using TurnBattle.Skills;
using TurnBattle.UI.Controls;

namespace TurnBattle.UI.Battle
{
	public class ActionSelectionUI : Panel
	{
		private BattleSystem _battleSystem;
		private NavigableCollection<UnitSlot> _units;
		private ScrollList _skillList;
		private Label _unitName;

		public void Show(IEnumerable<UnitSlot> units, BattleSystem battleSystem)
		{
			_battleSystem = battleSystem;
			_units = new NavigableCollection<UnitSlot>(units);
			_unitName = GetNode<Label>("Container/CurrentUnitContainer/Label");
			_skillList = GetNode<ScrollList>("Container/SkillList");
            var inputDeviceMap = ResourceLoader.Load<GamepadManagerResource>("res://resources//gamepad/GamepadManager.tres").GetActiveDeviceMap();
            var leftIcon = GetNode<TextureRect>("Container/CurrentUnitContainer/LeftIcon");
            leftIcon.Texture = inputDeviceMap.GetTextureForAction("ui_left");
            var rightIcon = GetNode<TextureRect>("Container/CurrentUnitContainer/RightIcon");
            rightIcon.Texture = inputDeviceMap.GetTextureForAction("ui_right");
			
            Visible = true;
			ChangeUnit(_units.Current);
		}

		public override void _Input(InputEvent inputEvent)
		{
            if(!Visible)
            {
                return;
            }
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
				CloseMenu();
			}
		}

		private void ChangeUnit(UnitSlot unitSlot)
		{
			var unit = (BattleUnit)unitSlot.Battler;
			_unitName.Text = unit.Name;
			_skillList.Clear();
			_skillList.AddRange(unit.Skills.Select(skill =>
			{
				var button = new SkillItem(skill);
				button.RectSize = new Vector2(100, 30);
				button.OnPressed += SelectSkill;
				return button;
			}));
			_skillList.FocusFirst();
		}

		private void SelectSkill(Skill skill)
		{
			var unit = _units.Current;
			ITargetGroup target = null;
			if(skill.TargetType == TargetTypeEnum.Multi)
			{
				target = new MultiTargetGroup(_battleSystem.Enemies.Where(x => !x.IsDead));
			}
			unit.SelectedAction.Select(skill, target);
			CloseMenu();
		}

		public void CloseMenu()
		{
			_battleSystem.SetInput(true);
			Hide();
		}
	}
}
