namespace TurnBattle.Godot.Battle.Actors
{
    public class BattleUnit : BaseBattler
    {
        public BattleUnit(UnitResource unit)
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
