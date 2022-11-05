using Godot;
using Rpg2d.Battle;
using Rpg2d.Battle.Actions;
using Rpg2d.Battle.Actors;
using System;
using System.Threading.Tasks;

namespace Rpg2d.Godot.Battle.Actors
{
    public class EnemySlot : Node2D, IBattlerSlot
    {
        private AnimatedSprite _animatedSprite;
        private BattleEnemy _enemy;
        private BattleAction _selectedAction = new BattleAction();
        public bool CanAct => !IsDead && ActionEnabled;
        public Action<BattleAction> ActionFinished { get; set; }
        public IBattler Battler => _enemy;
        public Action<SlotDamageRecivedArgs> DamageRecived { get; set; }
        public Action Died { get; set; }
        public bool IsActing { get; set; }

        private TaskCompletionSource<BattleAction> _actionTaskCompletionSource;

        public IActionDispatcher ActionDispatcher { get; set; }
        public bool ActionEnabled { get; set; }
        public bool IsDead { get; set; }
        private HitCounter _hitCounter = new HitCounter();

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
            _hitCounter.Init();
        }

        private void OnDied()
        {
            IsDead = true;
            _animatedSprite.Animation = "dead";
            Died?.Invoke();
        }

        private void OnDamageRecived(BattlerDamageRecivedArgs args)
        {
            DamageRecived?.Invoke(new SlotDamageRecivedArgs(this, args.Damage, _hitCounter.Hits));
        }

        public void PerformAction(BattleAction action)
        {
            ActionEnabled = false;
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

        public void DealDamage(int damage)
        {
            int hits = _hitCounter.CountHit();
            float hitModifier = 1 + 0.2f * (hits - 1);
            Battler.DealDamage((int)(damage * hitModifier));
        }
    }
}
