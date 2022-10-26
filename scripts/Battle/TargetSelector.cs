using Godot;
using System;
using System.Collections.Generic;

namespace Rpg2d.Battle
{
    public class TargetSelector : Node2D
    {
        private Node2D _selectionCursor;
        private List<EnemySlot> _enemies;
        private int _selectedIndex = 0;
        [Export]
        private bool _enabled;
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                if (_selectionCursor != null)
                {
                    _selectionCursor.Visible = _enabled;
                }
            }
        }

        public override void _Ready()
        {
            _selectionCursor = GetNode<Node2D>("SelectionArrow");
        }

        public void Init(List<EnemySlot> targets)
        {
            if (_selectionCursor == null)
            {
                _selectionCursor = GetNode<Node2D>("SelectionArrow");
            }
            _enemies = targets;
        }

        public void Select(int index)
        {
            _selectedIndex = index % _enemies.Count;
            var selectedEnemyPosition = _enemies[_selectedIndex].GetNode<AnimatedSprite>("AnimatedSprite");
            var position = selectedEnemyPosition.ToGlobal(new Vector2(0, -45));
            _selectionCursor.GlobalPosition = position;
        }
        public override void _Input(InputEvent inputEvent)
        {
            if (!Enabled) return;
            if (inputEvent.IsActionPressed("battle_enemy_up"))
            {
                Select(_selectedIndex + 1);
            }
            else if (inputEvent.IsActionPressed("battle_enemy_down"))
            {
                int offsetIndex = _selectedIndex - 1;
                if (offsetIndex < 0)
                {
                    offsetIndex = _enemies.Count - 1;
                }
                Select(offsetIndex);
            }
        }

        public EnemySlot GetSelected() => _enemies[_selectedIndex];
    }
}
