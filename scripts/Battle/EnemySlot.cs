using Godot;
using System;

namespace Rpg2d.Battle
{
    public class EnemySlot : Node
    {
        private AnimatedSprite _animatedSprite;
        private BattleEnemy _enemy;
        private BattleAction _selectedAction = new BattleAction();
        public bool CanAct { get; set; }
        public Action<BattleAction> ActionFinished { get; set; }
        public BattleEnemy Enemy => _enemy;

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

        public BattleAction Attack()
        {
            CanAct = false;
            _selectedAction.Select(_enemy.AttackSkill, null);
            _animatedSprite.Play(_selectedAction.Skill.ActionAnimation);
            _animatedSprite.Connect("animation_finished", this, nameof(ResetAnimation));
            return _selectedAction;
        }
    }
}
