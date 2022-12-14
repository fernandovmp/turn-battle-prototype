using TurnBattle.Battle.Actions;
using TurnBattle.Battle.Actors;
using TurnBattle.Godot.Skills;
using TurnBattle.Skills;
using System.Threading.Tasks;

namespace TurnBattle.Godot.Battle.Actors
{
    public class EnemySlot : BaseSlot
    {
        private BattleEnemy _enemy;
        public override IBattler Battler => _enemy;

        public void SetEnemy(EnemyResource enemyResource)
        {
            _enemy = new BattleEnemy(enemyResource);
            HasUnit = true;
            _animatedSprite.Frames = _enemy.AnimationFrames;
            _animatedSprite.Animation = _enemy.AttackSkill.IdleAnimation;
            _selectedAction.Reset(_enemy.AttackSkill);
            _enemy.DamageRecived += OnDamageRecived;
            _enemy.Died += OnDied;
            _hitCounter.Init();
            FlipSkillAnimation = false;
        }

        public override void PerformAction(BattleAction action)
        {
            ActionEnabled = false;
            IsActing = true;
            _actionTaskCompletionSource = new TaskCompletionSource<BattleAction>();
            ActionDispatcher.Dispatch(_actionTaskCompletionSource.Task);
            foreach (var target in action.TargetGroup.GetTargets())
            {
                var targetSlot = target as BaseSlot;
                var skillCaster = new SkillCaster(new CastContext
                {
                    Caster = action.Owner,
                    Skill = action.Skill,
                    Target = target
                });
                targetSlot.AddSkillAnimation(action.Skill.Animation, skillCaster);
            }
            _animatedSprite.Play(action.Skill.ActionAnimation);
            _animatedSprite.Connect(Constants.AnimationFinishedSignal, this, nameof(ResetAnimation));
        }

        private void ResetAnimation()
        {
            _animatedSprite.Disconnect(Constants.AnimationFinishedSignal, this, nameof(ResetAnimation));
            IsActing = false;
            ActionFinished?.Invoke(_selectedAction);
            _actionTaskCompletionSource.SetResult(_selectedAction);
            _selectedAction.Reset(_enemy.AttackSkill);
            _animatedSprite.Animation = _enemy.AttackSkill.IdleAnimation;
        }
    }
}
