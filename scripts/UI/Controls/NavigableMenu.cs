using System;
using System.Collections.Generic;
using Godot;

namespace TurnBattle.UI.Controls
{
    public class NavigableMenu
    {
        public IMenu Current { get; private set; }
        public Stack<IMenu> Previous { get; private set; } = new Stack<IMenu>();

        public void NavigateTo(IMenu menu)
        {
            Current?.Hide();
            Previous.Push(Current);
            Current = menu;
            Open(Current);
        }

        public void GoBack()
        {
            if(Previous.Count > 0)
            {
                Current.Hide();
                Current = Previous.Pop();
                Open(Current);
            }
        }

        private void Open(IMenu menu)
        {
            if(menu != null)
            {
                menu.Open();
                menu.OnReady?.Invoke();
            }
        }
    }
}