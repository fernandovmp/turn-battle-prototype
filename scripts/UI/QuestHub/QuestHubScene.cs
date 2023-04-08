using System;
using System.Collections.Generic;
using Godot;
using TurnBattle.Godot.Battle;
using TurnBattle.Godot.Battle.Actors;
using TurnBattle.Godot.Extensions;
using TurnBattle.Godot.Quests;
using TurnBattle.Services;
using TurnBattle.UI.Controls;

namespace TurnBattle.UI.QuestHub
{
	public class QuestHubScene : Node
	{
		[Export]
		private QuestResource[] _quests;
		[Export]
		private PackedScene _questItemModel;
		[Export]
		private UnitResource _partyLeftUnit;
		[Export]
		private UnitResource _partyUpUnit;
		[Export]
		private UnitResource _partyRightUnit;
		[Export]
		private UnitResource _partyBottomUnit;
		[Export]
		private PackedScene _controlItemModel;
		private ScrollList _questContainer;
		private Control _questListContainer;
		private ScrollContainer _creditsContainer;
        private ScrollContainer _controlsContainer;
        private Button _creditsButton;
        private NavigableMenu _navigableMenu;
        private const int ScrollSpeed = 50;

		public override void _Ready()
		{
			_questContainer = GetNode<ScrollList>("CanvasLayer/QuestScrollContainer");
			var questList = new List<QuestItem>();
			_creditsContainer = GetNode<ScrollContainer>("CanvasLayer/CreditsContainer");
			_controlsContainer = GetNode<ScrollContainer>("CanvasLayer/ControlsContainer");
			MakeMenu();
			foreach (var quest in _quests)
			{
				var questItem = _questItemModel.Instance<QuestItem>();
				questItem.SetQuest(quest);
				questItem.OnPressed += StartQuest;
				questList.Add(questItem);
			}
			MakeControlList();
			_questContainer.AddRange(questList);
		}

        private void MakeControlList()
        {
			Func<string, string> actionText = (unit) => $"(Battle) Attack with {unit} unit";
            var controls = new List<Tuple<string, string>>()
			{
				new Tuple<string, string>("battle_open_skill_ui", "(Battle) Open the skill menu to change a unit action"),
				new Tuple<string, string>("battle_enemy_up", "(Battle) Change the target"),
				new Tuple<string, string>("battle_enemy_down", "(Battle) Change the target"),
				new Tuple<string, string>("battle_left_unit_action", actionText("left")),
				new Tuple<string, string>("battle_up_unit_action", actionText("up")),
				new Tuple<string, string>("battle_right_unit_action", actionText("right")),
				new Tuple<string, string>("battle_bottom_unit_action", actionText("bottom")),
				new Tuple<string, string>("ui_accept", "(UI) Confirm"),
				new Tuple<string, string>("ui_cancel", "(UI) Cancel / Return")
			};
			var controlListContainer = _controlsContainer.GetNode("PanelContainer/ListContainer");
			foreach(var control in controls)
			{
				var controlItem = _controlItemModel.Instance<ControlItem>();
				controlItem.SetControl(control.Item1, control.Item2);
				controlListContainer.AddChild(controlItem);
			}
        }

        private void MakeMenu()
        {
			var rootScroll = GetNode<ScrollList>("CanvasLayer/RootMenu");
			_navigableMenu = new NavigableMenu();
			var questMenu = new GenericMenu(_questContainer, () => _questContainer.FocusFirst());
			var creditsMenu = new GenericMenu(_creditsContainer, ShowCredits);
			var controlsMenu = new GenericMenu(_controlsContainer);
			rootScroll.AddRange(new MenuItem[]
			{
				new MenuItem(_navigableMenu, questMenu, "Quests").WithSize(200, 50),
				new MenuItem(_navigableMenu, controlsMenu, "Controls").WithSize(200, 50),
				new MenuItem(_navigableMenu, creditsMenu, "Credits").WithSize(200, 50)
			});
            var rootMenu = new GenericMenu(rootScroll, () => rootScroll.FocusFirst());
			_navigableMenu.NavigateTo(rootMenu);
        }

		private void ShowCredits()
		{
			var file = new File();
			var credits = file.ReadAllText("res://credits.txt");
			var label = _creditsContainer.GetChild(0).GetChild<Label>(0);
			label.Text = credits;
		}

		public override void _Input(InputEvent @event)
		{
			if(@event.IsActionPressed("ui_cancel"))
			{
				_navigableMenu.GoBack();
			}
			else if(_creditsContainer?.Visible == true)
			{
				HandleCreditsInput(@event);
			}
		}

		private void HandleCreditsInput(InputEvent @event)
		{
			if(@event.IsActionPressed("ui_down", allowEcho: true))
			{
				_creditsContainer.ScrollVertical += ScrollSpeed;
			}
			else if(@event.IsActionPressed("ui_up", allowEcho: true))
			{
				_creditsContainer.ScrollVertical -= ScrollSpeed;
			}
		}

		private void StartQuest(QuestResource quest)
		{
			var repository = new MemoryCacheRepository();
			repository.SetValue(TurnBattle.Godot.Battle.Constants.BattleContextKey, new BattleSystemContext
			{
				PartyLeftUnit = _partyLeftUnit,
				PartyUpUnit = _partyUpUnit,
				PartyRightUnit = _partyRightUnit,
				PartyBottomUnit = _partyBottomUnit,
				Troop = quest.Troop
			});
			repository.SetValue(TurnBattle.Godot.Battle.Constants.BattleBackgroundKey, quest.Background);
			repository.SetValue(TurnBattle.Godot.Battle.Constants.BattleMusicKey, quest.BattleMusic);
			GetTree().ChangeScene("res://scenes/battle.tscn");
		}
	}
}
