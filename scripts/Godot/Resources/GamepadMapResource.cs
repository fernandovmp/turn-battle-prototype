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
    }
}