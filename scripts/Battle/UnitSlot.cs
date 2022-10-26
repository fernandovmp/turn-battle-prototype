using Godot;
using System;

namespace Rpg2d.Battle
{
    public class UnitSlot : Node
    {

        [Export]
        private string _actionMap;
        private AnimatedSprite _animatedSprite;
        private BattleUnit _unit;
        private BattleAction _selectedAction = new BattleAction();
        public bool CanAct { get; set; }
        public Action<BattleAction> ActionFinished { get; set; }

        public override void _Ready()
        {
            _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        }

        public override void _Input(InputEvent inputEvent)
        {
            if (inputEvent.IsActionPressed(_actionMap) && CanAct)
            {
                CanAct = false;
                _animatedSprite.Play(_selectedAction.Skill.ActionAnimation);
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
        }
    }
}
