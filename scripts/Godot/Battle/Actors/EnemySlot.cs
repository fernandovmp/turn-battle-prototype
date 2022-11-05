using Rpg2d.Battle.Actions;
using Rpg2d.Battle.Actors;
using System.Threading.Tasks;

namespace Rpg2d.Godot.Battle.Actors
{
    public class EnemySlot : BaseSlot
    {
        private BattleEnemy _enemy;
        public override IBattler Battler => _enemy;

        public void SetEnemy(EnemyResource enemyResource)
        {
            _enemy = new BattleEnemy(enemyResource);
            HasUnit = true;
            _animatedSprite.Frames = _enemy.AnimationFrames;
            _animatedSprite.Animation = _enemy.AttackSkill.IdleAnimation;
            _selectedAction.Reset(_enemy.AttackSkill);
            _enemy.DamageRecived += OnDamageRecived;
            _enemy.Died += OnDied;
            _hitCounter.Init();
        }

        public override void PerformAction(BattleAction action)
        {
            ActionEnabled = false;
            IsActing = true;
            _actionTaskCompletionSource = new TaskCompletionSource<BattleAction>();
            ActionDispatcher.Dispatch(_actionTaskCompletionSource.Task);
            _animatedSprite.Play(action.Skill.ActionAnimation);
            _animatedSprite.Connect(Constants.AnimationFinishedSignal, this, nameof(ResetAnimation));
        }

        private void ResetAnimation()
        {
            _animatedSprite.Disconnect(Constants.AnimationFinishedSignal, this, nameof(ResetAnimation));
            IsActing = false;
            ActionFinished?.Invoke(_selectedAction);
            _actionTaskCompletionSource.SetResult(_selectedAction);
            _selectedAction.Reset(_enemy.AttackSkill);
            _animatedSprite.Animation = _enemy.AttackSkill.IdleAnimation;
        }
    }
}
