using System;
using Rpg2d.Battle.Actors;

namespace Rpg2d.Battle
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