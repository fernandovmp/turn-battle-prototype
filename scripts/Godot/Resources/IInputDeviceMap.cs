using Godot;

namespace Rpg2d.Godot.Resources
{
    public interface IInputDeviceMap
    {
        Texture GetTextureForEvent(object @event);
    }
}