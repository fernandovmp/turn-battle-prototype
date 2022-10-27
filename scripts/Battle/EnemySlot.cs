using Godot;
using System;

namespace Rpg2d.Battle
{
    public class EnemySlot : Node, IBattlerSlot
    {
        private AnimatedSprite _animatedSprite;
        private BattleEnemy _enemy;
        private BattleAction _selectedAction = new BattleAction();
        public bool CanAct { get; set; }
        public Action<BattleAction> ActionFinished { get; set; }
        public IBattler Battler => _enemy;

        public override void _Ready()
        {
            _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        }

        private void ResetAnimation()
        {
            _animatedSprite.Disconnect("animation_finished", this, nameof(ResetAnimation));
            ActionFinished?.Invoke(_selectedAction);
            _selectedAction.Reset(_enemy.AttackSkill);
            _animatedSprite.Animation = _enemy.AttackSkill.IdleAnimation;
        }

        public void SetEnemy(EnemyResource enemyResource)
        {
            _enemy = new BattleEnemy(enemyResource);
            _animatedSprite.Frames = _enemy.AnimationFrames;
            _animatedSprite.Animation = _enemy.AttackSkill.IdleAnimation;
            _selectedAction.Reset(_enemy.AttackSkill);
        }

        public void PerformAction(BattleAction action)
        {
            CanAct = false;
            _animatedSprite.Play(action.Skill.ActionAnimation);
            _animatedSprite.Connect("animation_finished", this, nameof(ResetAnimation));
        }
    }
}
