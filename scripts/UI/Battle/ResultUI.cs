using Godot;
using TurnBattle.Godot.Resources;

namespace TurnBattle.UI.Battle
{
    public class ResultUI : Panel
    {
        public bool InputEnabled { get; private set; }

        public void ShowResult(string title)
        {
            Visible = true;
            InputEnabled = true;
            GetNode<Label>("TitleLabel").Text = title;
            var inputDeviceMap = ResourceLoader.Load<GamepadManagerResource>("res://resources//gamepad/GamepadManager.tres").GetActiveDeviceMap();
            var continueLabel = GetNode<RichTextLabel>("ContinueLabel");
            continueLabel.BbcodeText = "";
            continueLabel.PushAlign(RichTextLabel.Align.Center);
            continueLabel.AddText("Press ");
            Texture texture = inputDeviceMap.GetTextureForAction("ui_accept");
            continueLabel.AddImage(texture, width: ToProportionaly(texture.GetWidth(), texture.GetHeight(), 24), height: 24);
            continueLabel.AddText(" to continue");
        }

        private int ToProportionaly(int size, int originalRef, int reference)
        {
            return size / (originalRef / reference);
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
