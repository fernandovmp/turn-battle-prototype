using Godot;

namespace TurnBattle.Godot.Resources
{
    public interface IInputDeviceMap
    {
        Texture GetTextureForEvent(object @event);
    }
}