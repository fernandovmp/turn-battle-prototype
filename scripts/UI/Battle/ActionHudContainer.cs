using System;
using Godot;
using TurnBattle.Battle.Actors;
using TurnBattle.Godot.Battle.Actors;
using TurnBattle.Godot.Resources;

namespace TurnBattle.UI.Battle
{
    public class ActionHudContainer : Container
    {
        private bool _initialized;
        private Label _actionLabel;
        private TextureRect _actionButton;
        private Panel _background;
        private HBoxContainer _boxContainer;
        private UnitSlot _unit;
        public DirectionEnum Direction { get; set; } = DirectionEnum.Left;

        public override void _Ready()
        {
            Initialize();
        }

        public void Initialize()
        {
            if(_initialized)
            {
                return;
            }
            _background = GetNode<Panel>("Background");
            _boxContainer = GetNode<HBoxContainer>("BoxContainer");
            _actionLabel = _boxContainer.GetNode<Label>("Label");
            _actionButton = _boxContainer.GetNode<TextureRect>("ActionButton");
            _initialized = true;
        }

        public override void _Notification(int what)
        {
            switch(what)
            {
                case NotificationSortChildren:
                    HandleResize();
                    HandleDirection();
                    break;
                case NotificationResized:
                    HandleResize();
                    break;
                default:
                    break;
            }
        }

        private void HandleResize()
        {
            _background.SetAnchorsPreset(LayoutPreset.BottomWide);
            _background.RectMinSize = new Vector2(RectSize.x, RectSize.y / 2);
            _background.MarginBottom = 0;
            _background.MarginRight = 0;
            _background.MarginLeft = 0;
            _background.MarginTop = 0;
            _boxContainer.RectSize = RectSize;
            _boxContainer.SetAnchorsPreset(LayoutPreset.Wide);
            float minSize = Mathf.Min(RectSize.x, RectSize.y);
            _actionButton.RectMinSize = new Vector2(minSize, minSize);
        }

        private void HandleDirection()
        {
            int position;
            Label.AlignEnum labelAlign;
            if(Direction == DirectionEnum.Left)
            {
                position = 1;
                labelAlign = Label.AlignEnum.Right;
            }
            else
            {
                position = 0;
                labelAlign = Label.AlignEnum.Left;
            }
            _boxContainer.MoveChild(_actionButton, position);
            _actionLabel.Align = labelAlign;
        }

        public void SetUnit(UnitSlot unit)
        {
            _unit = unit;
            _unit.Died += OnUnitDied;
            _actionLabel.Text = _unit.SelectedAction.Skill.Name;
            var inputDeviceMap = ResourceLoader.Load<GamepadManagerResource>("res://resources//gamepad/GamepadManager.tres").GetActiveDeviceMap();
            _actionButton.Texture = inputDeviceMap.GetTextureForAction(_unit.ActionMap);
        }

        private void OnUnitDied()
        {
            Visible = false;
        }

        public enum DirectionEnum
        {
            Left,
            Right
        }
    }
}
