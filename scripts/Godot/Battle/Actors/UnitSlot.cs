using Godot;
using TurnBattle.Battle.Actions;
using TurnBattle.Battle.Actors;
using TurnBattle.Godot.Skills;
using TurnBattle.Skills;
using System;
using System.Threading.Tasks;

namespace TurnBattle.Godot.Battle.Actors
{
    public class UnitSlot : BaseSlot
    {
        [Export]
        private string _actionMap;
        private BattleUnit _unit;
        public override IBattler Battler => _unit;
        private TargetSelector _targetSelector;
        private SkillCaster _skillCaster;
        public BattleAction SelectedAction => _selectedAction;
        public string ActionMap => _actionMap;

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
            
            _actionTaskCompletionSource = new TaskCompletionSource<BattleAction>();
            if (action.TargetGroup is null)
            {
                action.TargetGroup = new SingleTargetGroup(_targetSelector.GetSelected());
            }
            if(action.CannotExceute())
            {
                return;
            }
            ActionEnabled = false;
            IsActing = true;
            action.Execute();
            _animatedSprite.Connect(Constants.AnimationFinishedSignal, this, nameof(ResetAnimation));
            _animatedSprite.Play(action.Skill.ActionAnimation);
            ActionDispatcher.Dispatch(_actionTaskCompletionSource.Task);
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
            FlipSkillAnimation = true;
        }
    }
}
