using Godot;
using Rpg2d.Battle;
using System;
using System.Collections.Generic;

namespace Rpg2d.UI.Battle
{
    public class ResultUI : Panel
    {
        public bool InputEnabled { get; private set; }

        public void ShowResult(string title)
        {
            Visible = true;
            InputEnabled = true;
            GetNode<Label>("TitleLabel").Text = title;
        }

        public override void _Input(InputEvent @event)
        {
            if (InputEnabled && @event.IsActionPressed("ui_accept"))
            {
                GetTree().ChangeScene("res://scenes/quest_hub.tscn");
            }
        }

        public void HideResult()
        {
            Visible = false;
            InputEnabled = false;
        }
    }
}
