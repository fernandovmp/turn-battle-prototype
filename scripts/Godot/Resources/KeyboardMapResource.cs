using Godot;

namespace TurnBattle.Godot.Resources
{

    public class KeyboardMapResource : Resource, IInputDeviceMap
    {
        [Export]
        public string Name { get; set; }
        [Export]
        public Texture A { get; set; }
        [Export]
        public Texture W { get; set; }
        [Export]
        public Texture S { get; set; }
        [Export]
        public Texture D { get; set; }
        [Export]
        public Texture Space { get; set; }
        [Export]
        public Texture Left { get; set; }
        [Export]
        public Texture Right { get; set; }

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
                case KeyList.Left:
                    return Left;
                case KeyList.Right:
                    return Right;
                default:
                    return null;
            }
        }
    }
}