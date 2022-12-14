using System;
using System.Collections.Generic;
using Godot;
using TurnBattle.Godot.Battle;
using TurnBattle.Godot.Battle.Actors;
using TurnBattle.Godot.Extensions;
using TurnBattle.Godot.Quests;
using TurnBattle.Services;

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
        private Control _questListContainer;
        private ScrollContainer _creditsContainer;
        private Button _creditsButton;
        private const int ScrollSpeed = 50;

        public override void _Ready()
        {
            _questListContainer = GetNode<Control>("CanvasLayer/QuestScrollContainer/QuestList");
            var questList = new List<QuestItem>();
            _creditsContainer = GetNode<ScrollContainer>("CanvasLayer/CreditsContainer");
            _creditsButton = GetNode<Button>("CanvasLayer/CreditsButton");
            _creditsButton.Connect("pressed", this, nameof(ShowCredits));
            foreach (var quest in _quests)
            {
                var questItem = _questItemModel.Instance<QuestItem>();
                questItem.SetQuest(quest);
                _questListContainer.AddChild(questItem);
                questItem.OnPressed += StartQuest;
                questList.Add(questItem);
                questItem.FocusNeighbourRight = _creditsButton.GetPath();
            }
            if (questList.Count > 1)
            {
                questList[0].FocusNeighbourTop = questList[questList.Count - 1].GetPath();
                questList[0].FocusNeighbourBottom = questList[1].GetPath();
                questList[questList.Count - 1].FocusNeighbourBottom = questList[0].GetPath();
                questList[questList.Count - 1].FocusNeighbourTop = questList[questList.Count - 2].GetPath();
            }
            for (int i = 1; i < questList.Count - 1; i++)
            {
                questList[i].FocusNeighbourBottom = questList[i + 1].GetPath();
                questList[i].FocusNeighbourTop = questList[i - 1].GetPath();
            }
            if (questList.Count > 0)
            {
                _creditsButton.FocusNeighbourLeft = questList[0].GetPath();
                questList[0].GrabFocus();
            }
        }

        private void ShowCredits()
        {
            ShowCredits(show: true);
        }

        private void ShowCredits(bool show)
        {
            if(show)
            {
                var file = new File();
                var credits = file.ReadAllText("res://credits.txt");
                var label = _creditsContainer.GetChild<Label>(0);
                label.Text = credits;
            }
            _creditsContainer.Visible = show;
            _creditsButton.Visible = !show;
            _questListContainer.Visible = !show;
        }

        public override void _Input(InputEvent @event)
        {
            if(_creditsContainer?.Visible == true)
            {
                HandleCreditsInput(@event);
            }
        }

        private void HandleCreditsInput(InputEvent @event)
        {
            if(@event.IsActionPressed("ui_cancel"))
            {
                ShowCredits(show: false);
                var quest = _questListContainer.GetChildOrNull<QuestItem>(0);
                quest?.GrabFocus();
            }
            else if(@event.IsActionPressed("ui_down", allowEcho: true))
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