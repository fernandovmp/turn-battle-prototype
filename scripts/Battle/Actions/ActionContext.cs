using System.Collections.Generic;
using Rpg2d.Battle.Actors;

namespace Rpg2d.Battle.Actions
{
    public class ActionContext
    {
        public IEnumerable<IBattlerSlot> Enemies { get; set; }
        public IEnumerable<IBattlerSlot> Party { get; set; }
    }
}