using Godot;

namespace TurnBattle.Godot.Resources
{

    public class KeyboardMapResource : Resource, IInputDeviceMap
    {
        [Export]
        public string Name { get; set; }
        [Export]
        public ImageTexture A { get; set; }
        [Export]
        public ImageTexture W { get; set; }
        [Export]
        public ImageTexture S { get; set; }
        [Export]
        public ImageTexture D { get; set; }
        [Export]
        public ImageTexture Space { get; set; }

        public Texture GetTextureForEvent(object @event)
        {
            if(@event is InputEventKey key)
            {
                return GetKeyTexture((KeyList)key.Scancode);
            }
            return null;
        }

        public Texture GetKeyTexture(KeyList keyCode)
        {
            switch(keyCode)
            {
                case KeyList.A:
                    return A;
                case KeyList.W:
                    return W;
                case KeyList.S:
                    return S;
                case KeyList.D:
                    return D;
                case KeyList.Space:
                    return Space;
                default:
                    return null;
            }
        }
    }
}