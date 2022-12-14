using System;
using TurnBattle.Battle.Actors;

namespace TurnBattle.Battle
{
    public interface ITargetSelector
    {
        void First();
        void Next();
        void Previous();
        IBattlerSlot GetSelected();
        Action<IBattlerSlot> SelectedTargetChanged { get; set; }
    }
}