using Godot;
using TurnBattle.Battle;
using TurnBattle.Battle.Actions;
using TurnBattle.Battle.Actors;
using TurnBattle.Godot.Skills;
using System;
using System.Threading.Tasks;
using TurnBattle.Skills;

namespace TurnBattle.Godot.Battle.Actors
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
        public bool FlipSkillAnimation { get; protected set; }

        protected HitCounter _hitCounter = new HitCounter(Constants.HitMillisecondsTolerance);
        protected Node _skillAnimationRoot;

        public override void _Ready()
        {
            _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
            _selectedAction.Owner = this;
            _skillAnimationRoot = GetNode<Node>("SkillAnimationRoot");
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
            DamageRecived?.Invoke(new SlotDamageRecivedArgs(this, args.Damage, _hitCounter.Hits));
        }

        public void DealDamage(int damage)
        {
            int hits = _hitCounter.CountHit();
            float hitModifier = 1 + 0.08f * (hits - 1);
            Battler.DealDamage((int)Math.Ceiling(damage * hitModifier));
        }

        public void HandleSkillAsTarget(CastContext castContext) => AddSkillAnimation(castContext.Skill.Animation, new SkillCaster(castContext));

        public void AddSkillAnimation(SkillAnimation animation, SkillCaster skillCaster)
        {
            var animatedSkill = new SkillAnimatedSprite();
            animatedSkill.FlipH = FlipSkillAnimation;
            animatedSkill.FrameChanged = skillCaster.OnFrame;
            skillCaster.Hited += animatedSkill.OnHit;
            _skillAnimationRoot.AddChild(animatedSkill);
            animatedSkill.Play(animation);
        }
    }
}
