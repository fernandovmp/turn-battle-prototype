using System.Collections.Generic;
using Rpg2d.Battle;

namespace Rpg2d.Skills
{
    public class SingleTargetGroup : ITargetGroup
    {
        private readonly IBattlerSlot _target;
        public SingleTargetGroup(IBattlerSlot target)
        {
            _target = target;
        }

        public IEnumerable<IBattler> GetTargets()
        {
            yield return _target.Battler;
        }
    }
}
