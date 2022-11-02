using System;
using Godot;
using Rpg2d.Quests;

namespace Rpg2d.UI.QuestHub
{
    public class QuestItem : Button
    {
        public QuestResource Quest { get; private set; }
        public Action<QuestResource> OnPressed { get; set; }

        public override void _Ready()
        {
            Text = Quest.Name;
        }


        public void SetQuest(QuestResource quest)
        {
            Quest = quest;
        }

        public override void _Pressed()
        {
            base._Pressed();
            OnPressed?.Invoke(Quest);
        }
    }
}