using System;

namespace Rpg2d.Battle
{
    public interface ITargetSelector
    {
        void First();
        void Next();
        void Previous();
        IBattler GetSelected();
        Action<IBattlerSlot> SelectedTargetChanged { get; set; }
    }
}