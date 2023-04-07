using System;
using Godot;

namespace TurnBattle.UI.Controls
{
    public interface IMenu
    {
        Action OnReady { get; }
        void Hide();
        void Open();
    }
}