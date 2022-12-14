using System.Collections.Generic;
using TurnBattle.Battle.Actions;

namespace TurnBattle.Battle.AI
{
    public interface IBattleAI
    {
        void Init(ActionContext context);
        IEnumerator<BattleAction> GetActions(ActionContext context);
    }
}