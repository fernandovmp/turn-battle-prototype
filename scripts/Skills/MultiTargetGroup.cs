using System.Collections.Generic;
using Rpg2d.Battle.Actors;

namespace Rpg2d.Skills
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
