using Godot;
using Rpg2d.Battle;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Rpg2d.Skills
{
    public interface ITargetGroup
    {
        IEnumerable<IBattlerSlot> GetTargets();
    }
}
