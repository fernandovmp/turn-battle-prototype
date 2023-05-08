using Godot;

namespace TurnBattle.Godot.Resources
{

    public class GamepadMapResource : Resource, IInputDeviceMap
    {
        [Export]
        public string Name { get; set; }
        [Export]
        public Texture BottomDigitalButton { get; set; }
        [Export]
        public Texture LeftDigitalButton { get; set; }
        [Export]
        public Texture TopDigitalButton { get; set; }
        [Export]
        public Texture RightDigitalButton { get; set; }
        [Export]
        public Texture LeftArrow { get; set; }
        [Export]
        public Texture UpArrow { get; set; }
        [Export]
        public Texture RightArrow { get; set; }
        [Export]
        public Texture DownArrow { get; set; }
        [Export]
        public Texture Start { get; set; }

        public Texture GetTextureForEvent(object @event)
        {
            if(@event is InputEventJoypadButton joypadButton)
            {
                return GetActionButtonTexture(joypadButton.ButtonIndex);
            }
            return null;
        }

        public Texture GetActionButtonTexture(int buttonIndex)
        {
            switch(buttonIndex)
            {
                case 0:
                    return BottomDigitalButton;
                case 1:
                    return RightDigitalButton;
                case 2:
                    return LeftDigitalButton;
                case 3:
                    return TopDigitalButton;
                case 11:
                    return Start;
                case 12:
                    return UpArrow;
                case 13:
                    return DownArrow;
                case 14:
                    return LeftArrow;
                case 15:
                    return RightArrow;
                default:
                    return null;
            }
        }
    }
}