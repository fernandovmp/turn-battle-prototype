using Godot;
using Rpg2d.Services;

namespace Rpg2d.Battle
{
    public class BattleScene : Node
    {
        public override void _Ready()
        {
            var battleSystem = GetNode<BattleSystem>("BattleSystem");
            var repository = new MemoryCacheRepository();
            var context = repository.GetValue<BattleSystemContext>("battle_context");
            battleSystem.Init(context);
        }
    }
}