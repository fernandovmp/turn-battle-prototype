using Godot;
using Rpg2d.UI.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rpg2d.Battle
{
    public class ActionDispatcher : IActionDispatcher
    {
        private List<Task<BattleAction>> _actions = new List<Task<BattleAction>>();
        private object _locker = new object();
        private bool _executingAction;
        public Action AllActionsFinished { get; set; }

        public void Dispatch(Task<BattleAction> action)
        {
            lock (_locker)
            {
                _actions.Add(action);
                if (!_executingAction)
                {
                    _executingAction = true;
                    WaitAllActions();
                }
            }
        }

        private async void WaitAllActions()
        {
            while (_actions.Any(t => !t.IsCompleted))
            {
                await Task.WhenAll(_actions);
            }
            _actions.Clear();
            _executingAction = false;
            AllActionsFinished?.Invoke();
        }
    }
}