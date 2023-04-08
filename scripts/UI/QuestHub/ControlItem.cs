using Godot;
using TurnBattle.Godot.Resources;

namespace TurnBattle.UI.QuestHub
{
    public class ControlItem : Control
    {
        public string Action { get; private set; }
        public string Description { get; private set; }

        public override void _Ready()
        {
            var inputDeviceMap = ResourceLoader.Load<GamepadManagerResource>("res://resources//gamepad/GamepadManager.tres").GetActiveDeviceMap();
            Texture texture = inputDeviceMap.GetTextureForAction(Action);
            GetNode<TextureRect>("Icon").Texture = texture;
            GetNode<Label>("Label").Text = Description;
        }


        public void SetControl(string action, string description)
        {
            Action = action;
            Description = description;
        }
    }
}