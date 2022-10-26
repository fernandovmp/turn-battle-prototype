using System.Collections.Generic;

namespace Rpg2d.Battle
{
    public class BattleContext
    {
        public IEnumerable<IBattler> Enemies { get; set; }
        public IEnumerable<IBattler> Party { get; set; }
    }
}