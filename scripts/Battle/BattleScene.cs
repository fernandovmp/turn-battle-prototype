using Godot;
using Rpg2d.Services;
using Rpg2d.UI.Battle;

namespace Rpg2d.Battle
{
    public class BattleScene : Node
    {
        public override void _Ready()
        {
            var battleUiController = GetNode<BattleUiController>("CanvasLayer");
            var battleSystem = GetNode<BattleSystem>("BattleSystem");
            var repository = new MemoryCacheRepository();
            var context = repository.GetValue<BattleSystemContext>("battle_context");
            battleUiController.SetBattleSystem(battleSystem);
            battleSystem.Init(context);
        }
    }
}