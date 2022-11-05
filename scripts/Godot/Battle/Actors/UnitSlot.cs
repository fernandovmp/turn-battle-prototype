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

        public override void _Ready()
        {
            base._Ready();
            _targetSelector = GetNode<TargetSelector>("/root/Root/TargetSelector");
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
                _animatedSprite.Connect(Constants.AnimationFrameEnd, this, nameof(OnFrame));
            }
            _animatedSprite.Connect(Constants.AnimationFinishedSignal, this, nameof(ResetAnimation));
            _animatedSprite.Play(action.Skill.ActionAnimation);
            ActionDispatcher.Dispatch(_actionTaskCompletionSource.Task);
        }

        private void OnFrame()
        {
            _skillCaster.OnFrame(_animatedSprite.Frame);
        }

        private void ResetAnimation()
        {
            IsActing = false;
            _animatedSprite.Disconnect(Constants.AnimationFinishedSignal, this, nameof(ResetAnimation));
            _animatedSprite.Disconnect(Constants.AnimationFrameEnd, this, nameof(OnFrame));
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
