using Godot;
using System;
using System.Collections;

namespace Rpg2d.Skills
{
    public interface ITargetGroup
    {
        IEnumerable GetTargets();
    }
}
