using System.Collections.Generic;
using Godot;
using Rpg2d.Godot.Battle;
using Rpg2d.Godot.Battle.Actors;
using Rpg2d.Godot.Quests;
using Rpg2d.Services;

namespace Rpg2d.UI.QuestHub
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

        public override void _Ready()
        {
            _questListContainer = GetNode<Control>("CanvasLayer/QuestScrollContainer/QuestList");
            var questList = new List<QuestItem>();
            foreach (var quest in _quests)
            {
                var questItem = _questItemModel.Instance<QuestItem>();
                questItem.SetQuest(quest);
                _questListContainer.AddChild(questItem);
                questItem.OnPressed += StartQuest;
                questList.Add(questItem);
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
                questList[0].GrabFocus();
            }
        }

        private void StartQuest(QuestResource quest)
        {
            var repository = new MemoryCacheRepository();
            repository.SetValue("battle_context", new BattleSystemContext
            {
                PartyLeftUnit = _partyLeftUnit,
                PartyUpUnit = _partyUpUnit,
                PartyRightUnit = _partyRightUnit,
                PartyBottomUnit = _partyBottomUnit,
                Troop = quest.Troop
            });
            GetTree().ChangeScene("res://scenes/battle.tscn");
        }
    }
}