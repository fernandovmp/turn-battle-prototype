using Godot;
using Rpg2d.Battle;
using Rpg2d.Battle.Actions;
using Rpg2d.Battle.Actors;
using System;
using System.Threading.Tasks;

namespace Rpg2d.Godot.Battle.Actors
{
    public abstract class BaseSlot : Node2D, IBattlerSlot
    {
        protected AnimatedSprite _animatedSprite;
        protected BattleAction _selectedAction = new BattleAction();
        public bool CanAct => !IsDead && ActionEnabled && HasUnit;
        public bool ActionEnabled { get; set; }
        public bool IsActing { get; set; }
        public bool IsDead { get; set; }
        public bool HasUnit { get; set; }
        public Action<BattleAction> ActionFinished { get; set; }
        public abstract IBattler Battler { get; }
        public Action<SlotDamageRecivedArgs> DamageRecived { get; set; }
        public Action Died { get; set; }
        protected TaskCompletionSource<BattleAction> _actionTaskCompletionSource;
        public IActionDispatcher ActionDispatcher { get; set; }
        protected HitCounter _hitCounter = new HitCounter(Constants.HitMillisecondsTolerance);

        public override void _Ready()
        {
            _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
            _selectedAction.Owner = this;
        }

        public abstract void PerformAction(BattleAction action);

        protected void OnDied()
        {
            _animatedSprite.Animation = Constants.DefaultDeadAnimation;
            IsDead = true;
            Died?.Invoke();
        }

        protected void OnDamageRecived(BattlerDamageRecivedArgs args)
        {
            DamageRecived?.Invoke(new SlotDamageRecivedArgs(this, args.Damage, 0));
        }

        public void DealDamage(int damage)
        {
            int hits = _hitCounter.CountHit();
            float hitModifier = 1 + 0.2f * (hits - 1);
            Battler.DealDamage((int)(damage * hitModifier));
        }
    }
}
