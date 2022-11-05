using Godot;
using Rpg2d.Battle.Actions;
using Rpg2d.Battle.AI;
using System.Collections.Generic;

namespace Rpg2d.Godot.Battle.AI
{
    public abstract class AiResource : Resource, IBattleAI
    {
        [Export]
        public string Name { get; set; }

        public abstract IEnumerator<BattleAction> GetActions(ActionContext context);
    }
}
