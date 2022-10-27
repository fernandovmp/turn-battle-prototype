using System.Collections.Generic;

namespace Rpg2d.Battle
{
    public class BattleContext
    {
        public IEnumerable<IBattlerSlot> Enemies { get; set; }
        public IEnumerable<IBattlerSlot> Party { get; set; }
    }
}