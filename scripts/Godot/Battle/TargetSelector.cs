using Godot;
using TurnBattle.Battle;
using TurnBattle.Battle.Actors;
using TurnBattle.Godot.Battle.Actors;
using System;
using System.Collections.Generic;

namespace TurnBattle.Godot.Battle
{
    public class TargetSelector : Node2D, ITargetSelector
    {
        private Node2D _selectionCursor;
        private List<EnemySlot> _enemies;
        private int _selectedIndex = 0;
        [Export]
        private bool _enabled;

        public Action<IBattlerSlot> SelectedTargetChanged { get; set; }
        public Action<bool> EnableChanged { get; set; }
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
                    EnableChanged?.Invoke(value);
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

        public override void _Input(InputEvent inputEvent)
        {
            if (!Enabled) return;
            if (inputEvent.IsActionPressed("battle_enemy_up"))
            {
                Next();
            }
            else if (inputEvent.IsActionPressed("battle_enemy_down"))
            {
                Previous();
            }
        }

        public IBattlerSlot GetSelected() => _enemies[_selectedIndex];

        public void First()
        {
            int index = _enemies.FindIndex(t => !t.IsDead);
            if (index < 0)
            {
                index = 0;
            }
            Select(index);
        }

        public void Next()
        {
            int index = (_selectedIndex + 1) % _enemies.Count;
            int startIndex = _selectedIndex;
            while (_enemies[index].IsDead && startIndex != index)
            {
                index = (index + 1) % _enemies.Count;
            }
            _selectedIndex = index;
            Select(index);
        }

        public void Previous()
        {
            int index = _selectedIndex - 1;
            if (index < 0)
            {
                index = _enemies.Count - 1;
            }
            int startIndex = _selectedIndex;
            while (_enemies[index].IsDead && startIndex != index)
            {
                index -= 1;
                if (index < 0)
                {
                    index = _enemies.Count - 1;
                }
            }
            _selectedIndex = index;
            Select(index);
        }

        private void Select(int index)
        {
            _selectedIndex = index % _enemies.Count;
            var selectedEnemyPosition = _enemies[_selectedIndex].GetNode<AnimatedSprite>("AnimatedSprite");
            var position = selectedEnemyPosition.ToGlobal(new Vector2(0, -45));
            _selectionCursor.GlobalPosition = position;
            SelectedTargetChanged?.Invoke(_enemies[_selectedIndex]);
        }
    }
}
