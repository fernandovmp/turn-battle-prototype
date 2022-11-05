using Rpg2d.Battle.Actors;
using System.Collections.Generic;

namespace Rpg2d.Skills
{
    public interface ITargetGroup
    {
        IEnumerable<IBattlerSlot> GetTargets();
    }
}
