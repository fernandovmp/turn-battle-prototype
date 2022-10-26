using Godot;
using System.Collections.Generic;

namespace Rpg2d.Battle
{
    public abstract class AiResource : Resource
    {
        [Export]
        public string Name { get; set; }

        public abstract IEnumerator<BattleAction> GetActions(BattleContext context);
    }
}
