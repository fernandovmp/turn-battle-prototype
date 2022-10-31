using Godot;
using Rpg2d.UI.Battle;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rpg2d.Battle
{
    public interface IActionDispatcher
    {
        Action AllActionsFinished { get; set; }
        void Dispatch(Task<BattleAction> action);
    }
}