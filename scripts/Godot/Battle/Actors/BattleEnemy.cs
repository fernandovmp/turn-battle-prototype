using TurnBattle.Battle.Stats;

namespace TurnBattle.Godot.Battle.Actors
{
    public class BattleEnemy : BaseBattler
    {
        public BattleEnemy(EnemyResource unit)
        {
            Name = unit.Name;
            Hp = new RangeStat(unit.MaxHp);
            Attack = unit.Attack;
            AnimationFrames = unit.AnimationFrames;
            AttackSkill = unit.AttackSkill.AsSkill();
        }
    }
}
