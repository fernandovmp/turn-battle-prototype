using System;
using Godot;

namespace TurnBattle.UI.Controls
{
    public class GenericMenu : IMenu
    {
        public Control Control { get; }

        public Action OnReady { get; private set; }

        public GenericMenu(Control control)
        {
            Control = control;
        }

        public GenericMenu(Control control, Action ready) : this(control)
        {
            OnReady = ready;
        }

        public void Hide() => Control.Hide();

        public void Open() => Control.Show();
    }
}