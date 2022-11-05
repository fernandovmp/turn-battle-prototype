using System.Collections.Generic;
using Rpg2d.Battle.Actions;

namespace Rpg2d.Battle.AI
{
    public interface IBattleAI
    {
        IEnumerator<BattleAction> GetActions(ActionContext context);
    }
}