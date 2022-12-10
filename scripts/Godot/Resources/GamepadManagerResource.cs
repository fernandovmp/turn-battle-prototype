using Godot;

namespace Rpg2d.Godot.Resources
{

    public class GamepadManagerResource : Resource
    {
        [Export]
        public GamepadMapResource XboxMap { get; set; }
        [Export]
        public GamepadMapResource PlaystationMap { get; set; }
        [Export]
        public KeyboardMapResource KeyboardMap { get; set; }

        public IInputDeviceMap GetActiveDeviceMap()
        {
            var gamepads = Input.GetConnectedJoypads();
            if(gamepads.Count > 0)
            {
                var gamepad = Input.GetJoyName((int)gamepads[0]).ToLowerInvariant();
                if(gamepad.Contains("xbox"))
                {
                    return XboxMap;
                }
                if(gamepad.Contains("ps5") || gamepad.Contains("ps4"))
                {
                    return PlaystationMap;
                }
                
            }
            return KeyboardMap;
        }
    }
}