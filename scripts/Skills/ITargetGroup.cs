using TurnBattle.Battle.Actors;
using System.Collections.Generic;

namespace TurnBattle.Skills
{
    public interface ITargetGroup
    {
        IEnumerable<IBattlerSlot> GetTargets();
    }
}
