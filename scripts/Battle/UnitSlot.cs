using Godot;
using System;

namespace Rpg2d.Battle
{
    public class UnitSlot : Node, IBattlerSlot
    {

        [Export]
        private string _actionMap;
        private AnimatedSprite _animatedSprite;
        private BattleUnit _unit;
        private BattleAction _selectedAction = new BattleAction();
        public bool CanAct { get; set; }
        public Action<BattleAction> ActionFinished { get; set; }
        public IBattler Battler => _unit;

        public Action DamageRecived { get; set; }
        public Action Died { get; set; }

        private TargetSelector _targetSelector;

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
                CanAct = false;
                _animatedSprite.Play(_selectedAction.Skill.ActionAnimation);
                if (_selectedAction.TargetGroup is null)
                {
                    _selectedAction.Skill.Cast(new Skills.CastContext
                    {
                        Caster = _unit,
                        Skill = _selectedAction.Skill,
                        Target = _targetSelector.GetSelected()
                    });
                }
                _animatedSprite.Connect("animation_finished", this, nameof(ResetAnimation));
            }
        }

        private void ResetAnimation()
        {
            _animatedSprite.Disconnect("animation_finished", this, nameof(ResetAnimation));
            ActionFinished?.Invoke(_selectedAction);
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
            Died?.Invoke();
        }

        private void OnDamageRecived()
        {
            DamageRecived?.Invoke();
        }

        public void PerformAction(BattleAction action)
        {
            throw new NotImplementedException();
        }
    }
}
