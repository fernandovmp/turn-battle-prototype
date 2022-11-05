using System;
using System.Threading.Tasks;

namespace Rpg2d.Battle.Actions
{
    public interface IActionDispatcher
    {
        Action AllActionsFinished { get; set; }
        void Dispatch(Task<BattleAction> action);
    }
}