using Godot;
using System;
using System.Threading.Tasks;

namespace Rpg2d.Battle
{
    public class EnemySlot : Node, IBattlerSlot
    {
        private AnimatedSprite _animatedSprite;
        private BattleEnemy _enemy;
        private BattleAction _selectedAction = new BattleAction();
        public bool CanAct { get; set; }
        public Action<BattleAction> ActionFinished { get; set; }
        public IBattler Battler => _enemy;
        public Action DamageRecived { get; set; }
        public Action Died { get; set; }
        public bool IsActing { get; set; }

        private TaskCompletionSource<BattleAction> _actionTaskCompletionSource;

        public IActionDispatcher ActionDispatcher { get; set; }
        public override void _Ready()
        {
            _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        }

        public void SetEnemy(EnemyResource enemyResource)
        {
            _enemy = new BattleEnemy(enemyResource);
            _animatedSprite.Frames = _enemy.AnimationFrames;
            _animatedSprite.Animation = _enemy.AttackSkill.IdleAnimation;
            _selectedAction.Reset(_enemy.AttackSkill);
            _enemy.DamageRecived += OnDamageRecived;
            _enemy.Died += OnDied;
        }

        private void OnDied()
        {
            Died?.Invoke();
        }

        private void OnDamageRecived()
        {
            DamageRecived?.Invoke();
        }

        public void PerformAction(BattleAction action)
        {
            CanAct = false;
            IsActing = true;
            _actionTaskCompletionSource = new TaskCompletionSource<BattleAction>();
            ActionDispatcher.Dispatch(_actionTaskCompletionSource.Task);
            _animatedSprite.Play(action.Skill.ActionAnimation);
            _animatedSprite.Connect("animation_finished", this, nameof(ResetAnimation));
        }

        private void ResetAnimation()
        {
            _animatedSprite.Disconnect("animation_finished", this, nameof(ResetAnimation));
            IsActing = false;
            ActionFinished?.Invoke(_selectedAction);
            _actionTaskCompletionSource.SetResult(_selectedAction);
            _selectedAction.Reset(_enemy.AttackSkill);
            _animatedSprite.Animation = _enemy.AttackSkill.IdleAnimation;
        }
    }
}
