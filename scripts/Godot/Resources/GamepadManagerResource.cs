using System.Linq;
using Godot;

namespace TurnBattle.Godot.Resources
{

    public class GamepadManagerResource : Resource
    {
        [Export]
        public GamepadMapResource XboxMap { get; set; }
        [Export]
        public GamepadMapResource PlaystationMap { get; set; }
        [Export]
        public KeyboardMapResource KeyboardMap { get; set; }

        private string[] _xboxNames = new string[]
        {
            "xbox",
            "x-box"
        };

        private string[] _playstationNames = new string[]
        {
            "ps4",
            "ps5",
            "sony"
        };

        public IInputDeviceMap GetActiveDeviceMap()
        {
            var gamepads = Input.GetConnectedJoypads();
            if(gamepads.Count > 0)
            {
                var gamepad = Input.GetJoyName((int)gamepads[0]).ToLowerInvariant();
                if(_playstationNames.Any(name => gamepad.Contains(name)))
                {
                    return PlaystationMap;
                }
                return XboxMap;
            }
            return KeyboardMap;
        }
    }
}