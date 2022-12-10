using Godot;

namespace Rpg2d.Godot.Resources
{
    public static class InputExtensions
    {
        public static Texture GetTextureForAction(this IInputDeviceMap inputDeviceMap, string actionName)
        {
            Texture texture;
            foreach(var inputEvent in InputMap.GetActionList(actionName))
            {
                texture = inputDeviceMap.GetTextureForEvent(inputEvent);
                if(texture != null)
                {
                    return texture;
                }
            }
            return null;
        }
    }
}