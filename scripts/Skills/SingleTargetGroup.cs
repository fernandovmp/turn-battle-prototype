using System.Collections.Generic;
using TurnBattle.Battle.Actors;

namespace TurnBattle.Skills
{
    public class SingleTargetGroup : ITargetGroup
    {
        private readonly IBattlerSlot _target;
        public SingleTargetGroup(IBattlerSlot target)
        {
            _target = target;
        }

        public IEnumerable<IBattlerSlot> GetTargets()
        {
            yield return _target;
        }
    }
}
