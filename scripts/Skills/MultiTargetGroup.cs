using System.Collections.Generic;
using TurnBattle.Battle.Actors;

namespace TurnBattle.Skills
{
    public class MultiTargetGroup : ITargetGroup
    {
        private readonly IEnumerable<IBattlerSlot> _targets;
        public MultiTargetGroup(IEnumerable<IBattlerSlot> targets)
        {
            _targets = targets;
        }

        public IEnumerable<IBattlerSlot> GetTargets() => _targets;
    }
}
