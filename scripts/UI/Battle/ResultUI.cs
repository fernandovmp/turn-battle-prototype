using Godot;
using Rpg2d.Godot.Resources;

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
            var gamepad = ResourceLoader.Load<GamepadMapResource>("res://resources//gamepad/XboxGamepad.tres");
            var continueLabel = GetNode<RichTextLabel>("ContinueLabel");
            continueLabel.BbcodeText = "";
            continueLabel.PushAlign(RichTextLabel.Align.Center);
            continueLabel.AddText("Press ");
            continueLabel.AddImage(gamepad.BottomDigitalButton, width: 24, height: 24);
            continueLabel.AddText(" to continue");
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
