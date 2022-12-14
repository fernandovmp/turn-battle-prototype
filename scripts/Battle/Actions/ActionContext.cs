using System.Collections.Generic;
using TurnBattle.Battle.Actors;

namespace TurnBattle.Battle.Actions
{
    public class ActionContext
    {
        public IEnumerable<IBattlerSlot> Enemies { get; set; }
        public IEnumerable<IBattlerSlot> Party { get; set; }
    }
}