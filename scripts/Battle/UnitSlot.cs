using Godot;
using System;
using System.Threading.Tasks;

namespace Rpg2d.Battle
{
    public class UnitSlot : Node, IBattlerSlot
    {

        [Export]
        private string _actionMap;
        private AnimatedSprite _animatedSprite;
        private BattleUnit _unit;
        private BattleAction _selectedAction = new BattleAction();
        public bool CanAct => !IsDead && ActionEnabled;
        public bool ActionEnabled { get; set; }
        public bool IsActing { get; set; }
        public bool IsDead { get; set; }
        public Action<BattleAction> ActionFinished { get; set; }
        public IBattler Battler => _unit;

        public Action<SlotDamageRecivedArgs> DamageRecived { get; set; }
        public Action Died { get; set; }

        private TargetSelector _targetSelector;
        private TaskCompletionSource<BattleAction> _actionTaskCompletionSource;
        public IActionDispatcher ActionDispatcher { get; set; }

        public override void _Ready()
        {
            _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
            _targetSelector = GetNode<TargetSelector>("/root/Root/TargetSelector");
            _selectedAction.Owner = this;
        }

        public override void _Input(InputEvent inputEvent)
        {
            if (inputEvent.IsActionPressed(_actionMap) && CanAct)
            {
                PerformAction(_selectedAction);
            }
        }

        public void PerformAction(BattleAction action)
        {
            ActionEnabled = false;
            IsActing = true;
            _actionTaskCompletionSource = new TaskCompletionSource<BattleAction>();
            _animatedSprite.Play(action.Skill.ActionAnimation);
            if (action.TargetGroup is null)
            {
                action.Skill.Cast(new Skills.CastContext
                {
                    Caster = this,
                    Skill = action.Skill,
                    Target = _targetSelector.GetSelected()
                });
            }
            _animatedSprite.Connect("animation_finished", this, nameof(ResetAnimation));
            ActionDispatcher.Dispatch(_actionTaskCompletionSource.Task);
        }

        private void ResetAnimation()
        {
            _animatedSprite.Disconnect("animation_finished", this, nameof(ResetAnimation));
            IsActing = false;
            ActionFinished?.Invoke(_selectedAction);
            _actionTaskCompletionSource.SetResult(_selectedAction);
            _selectedAction.Reset(_unit.AttackSkill);
            _animatedSprite.Animation = _unit.AttackSkill.IdleAnimation;
        }

        public void SetUnit(UnitResource unitResource)
        {
            _unit = new BattleUnit(unitResource);
            _animatedSprite.Frames = _unit.AnimationFrames;
            _animatedSprite.Animation = _unit.AttackSkill.IdleAnimation;
            _selectedAction.Reset(_unit.AttackSkill);
            _unit.DamageRecived += OnDamageRecived;
            _unit.Died += OnDied;
        }

        private void OnDied()
        {
            _animatedSprite.Animation = "dead";
            IsDead = true;
            Died?.Invoke();
        }

        private void OnDamageRecived(BattlerDamageRecivedArgs args)
        {
            DamageRecived?.Invoke(new SlotDamageRecivedArgs(this, args.Damage, 0));
        }

        public void DealDamage(int damage)
        {
            Battler.DealDamage(damage);
        }
    }
}
