using Godot;
using TurnBattle.Battle.Actions;
using TurnBattle.Battle.AI;
using System.Collections.Generic;

namespace TurnBattle.Godot.Battle.AI
{
    public abstract class AiResource : Resource, IBattleAI
    {
        [Export]
        public string Name { get; set; }

        public virtual void Init(ActionContext context) {}
        public abstract IEnumerator<BattleAction> GetActions(ActionContext context);
    }
}
