using Godot;
using Rpg2d.Battle.Actions;
using Rpg2d.Battle.Actors;
using Rpg2d.Godot.Skills;
using Rpg2d.Skills;
using System;
using System.Threading.Tasks;

namespace Rpg2d.Godot.Battle.Actors
{
    public class UnitSlot : BaseSlot
    {
        [Export]
        private string _actionMap;
        private BattleUnit _unit;
        public override IBattler Battler => _unit;
        private TargetSelector _targetSelector;
        private SkillCaster _skillCaster;
        private AnimatedSprite _skillAnimatedSprite;

        public override void _Ready()
        {
            base._Ready();
            _targetSelector = GetNode<TargetSelector>("/root/Root/TargetSelector");
            _skillAnimatedSprite = GetNode<AnimatedSprite>("SkillAnimatedSprite");
            _skillAnimatedSprite.Visible = false;
        }

        public override void _Input(InputEvent inputEvent)
        {
            if (inputEvent.IsActionPressed(_actionMap) && CanAct)
            {
                PerformAction(_selectedAction);
            }
        }

        public override void PerformAction(BattleAction action)
        {
            ActionEnabled = false;
            IsActing = true;
            _actionTaskCompletionSource = new TaskCompletionSource<BattleAction>();
            if (action.TargetGroup is null)
            {

                _skillCaster = new SkillCaster(new CastContext
                {
                    Caster = this,
                    Skill = action.Skill,
                    Target = _targetSelector.GetSelected()
                });
                _skillAnimatedSprite.Connect(Constants.AnimationFrameEnd, this, nameof(OnFrame));
                _skillAnimatedSprite.Connect(Constants.AnimationFinishedSignal, this, nameof(ResetSkillAnimation));
                _skillAnimatedSprite.Frames = action.Skill.Animation.Frames;
                _skillAnimatedSprite.Scale = action.Skill.Animation.CustomScale;
                _skillAnimatedSprite.Play(action.Skill.Animation.Animation);
                _skillAnimatedSprite.Visible = true;
            }
            _animatedSprite.Connect(Constants.AnimationFinishedSignal, this, nameof(ResetAnimation));
            _animatedSprite.Play(action.Skill.ActionAnimation);
            ActionDispatcher.Dispatch(_actionTaskCompletionSource.Task);
        }

        private void ResetSkillAnimation()
        {
            _skillAnimatedSprite.Disconnect(Constants.AnimationFrameEnd, this, nameof(OnFrame));
            _skillAnimatedSprite.Disconnect(Constants.AnimationFinishedSignal, this, nameof(ResetSkillAnimation));
            _skillAnimatedSprite.Frames = new SpriteFrames();
            _skillAnimatedSprite.Visible = false;
        }

        private void OnFrame()
        {
            _skillCaster.OnFrame(_skillAnimatedSprite.Frame);
        }

        private void ResetAnimation()
        {
            IsActing = false;
            _animatedSprite.Disconnect(Constants.AnimationFinishedSignal, this, nameof(ResetAnimation));
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
            HasUnit = true;
        }
    }
}
