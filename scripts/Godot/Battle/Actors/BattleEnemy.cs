namespace Rpg2d.Godot.Battle.Actors
{
    public class BattleEnemy : BaseBattler
    {
        public BattleEnemy(EnemyResource unit)
        {
            Name = unit.Name;
            MaxHp = unit.MaxHp;
            Hp = unit.MaxHp;
            Attack = unit.Attack;
            AnimationFrames = unit.AnimationFrames;
            AttackSkill = unit.AttackSkill.AsSkill();
        }
    }
}
