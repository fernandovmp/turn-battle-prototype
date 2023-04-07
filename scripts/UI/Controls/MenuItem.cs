using System;
using Godot;

namespace TurnBattle.UI.Controls
{
    public class MenuItem : Button
    {
        public MenuItem(NavigableMenu navigableMenu, IMenu nextMenu, string text)
        {
            Text = text;
            Menu = nextMenu;
            NavigableMenu = navigableMenu;
        }

        public IMenu Menu { get; private set; }
        public NavigableMenu NavigableMenu { get; private set; }

        public override void _Pressed()
        {
            base._Pressed();
            NavigableMenu.NavigateTo(Menu);
        }

        public MenuItem WithSize(int width, int height)
        {
            RectSize = new Vector2(width, height);
            RectMinSize = new Vector2(width, height);
            return this;
        }
    }
}