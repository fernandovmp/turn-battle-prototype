using System.Collections.Generic;
using Rpg2d.Battle;

namespace Rpg2d.Skills
{
    public class SingleTargetGroup : ITargetGroup
    {
        private readonly IBattler _target;
        public SingleTargetGroup(IBattler target)
        {
            _target = target;
        }

        public IEnumerable<IBattler> GetTargets()
        {
            yield return _target;
        }
    }
}
