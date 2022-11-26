using Godot;

namespace Rpg2d.Godot.Resources
{

    public class GamepadMapResource : Resource
    {
        [Export]
        public string Name { get; set; }
        [Export]
        public ImageTexture BottomDigitalButton { get; set; }
        [Export]
        public ImageTexture LeftDigitalButton { get; set; }
        [Export]
        public ImageTexture TopDigitalButton { get; set; }
        [Export]
        public ImageTexture RightDigitalButton { get; set; }

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
                default:
                    return null;
            }
        }
    }
}