using System.Collections.Generic;
using Godot;

namespace TurnBattle.UI.Controls
{
    public class ScrollList : ScrollContainer
    {
        private Control _listContainer;
        private List<Control> _items = new List<Control>();

        public override void _Ready()
        {
            base._Ready();
            _listContainer = GetNode<Control>("ListContainer");
            
        }

        public void AddRange(IEnumerable<Control> items)
        {
            foreach (var item in items)
            {
                _listContainer.AddChild(item);
                _items.Add(item);
            }
            if (_items.Count > 1)
            {
                _items[0].FocusNeighbourTop = _items[_items.Count - 1].GetPath();
                _items[0].FocusNeighbourBottom = _items[1].GetPath();
                _items[_items.Count - 1].FocusNeighbourBottom = _items[0].GetPath();
                _items[_items.Count - 1].FocusNeighbourTop = _items[_items.Count - 2].GetPath();
            }
            for (int i = 1; i < _items.Count - 1; i++)
            {
                _items[i].FocusNeighbourBottom = _items[i + 1].GetPath();
                _items[i].FocusNeighbourTop = _items[i - 1].GetPath();
            }
        }

        public void Clear()
        {
            foreach(var child in _items)
            {
                _listContainer.RemoveChild(child);
            }
            _items.Clear();
        }

        public void FocusFirst()
        {
            if(_items.Count > 0)
            {
                _items[0].GrabFocus();
            }
        }
    }
}