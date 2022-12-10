using Godot;
using Rpg2d.Services;
using Rpg2d.UI.Battle;

namespace Rpg2d.Godot.Battle
{
    public class BattleScene : Node
    {
        public override void _Ready()
        {
            var battleUiController = GetNode<BattleUiController>("CanvasLayer");
            var battleSystem = GetNode<BattleSystem>("BattleSystem");
            var battleBackground = GetNode<Sprite>("Background");
            var battleMusicPlayer = GetNode<AudioStreamPlayer>("AudioPlayer");
            var repository = new MemoryCacheRepository();
            var context = repository.GetValue<BattleSystemContext>(Constants.BattleContextKey);
            var backgroundTexture = repository.GetValue<Texture>(Constants.BattleBackgroundKey);
            var battleMusic = repository.GetValue<AudioStream>(Constants.BattleMusicKey);
            battleBackground.Texture = backgroundTexture;
            battleMusicPlayer.Stream = battleMusic;
            battleMusicPlayer.Play(0);
            battleUiController.SetBattleSystem(battleSystem);
            battleSystem.Init(context);
        }
    }
}