using System;
using System.Threading.Tasks;

namespace TurnBattle.Battle.Actions
{
    public interface IActionDispatcher
    {
        Action AllActionsFinished { get; set; }
        void Dispatch(Task<BattleAction> action);
    }
}