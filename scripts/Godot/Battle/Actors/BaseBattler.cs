using Godot;
using TurnBattle.Battle.Actors;
using TurnBattle.Skills;
using System;
using TurnBattle.Battle.Stats;

namespace TurnBattle.Godot.Battle.Actors
{
    public abstract class BaseBattler : IBattler
    {
        public string Name { get; set; }
        public RangeStat Hp { get; set; }
        public RangeStat Mp { get; set; }
        public int Attack { get; set; }
        public SpriteFrames AnimationFrames { get; set; }
        public Skill AttackSkill { get; set; }
        public Action<string> Update { get; set; }
        public Action<BattlerDamageRecivedArgs> DamageRecived { get; set; }
        public Action Died { get; set; }

        public void DealDamage(int damage)
        {
            Hp -= damage;
            if (Hp == 0)
            {
                Died?.Invoke();
            }
            Update?.Invoke(nameof(Hp));
            DamageRecived?.Invoke(new BattlerDamageRecivedArgs
            {
                Damage = damage
            });
        }

        public void UseMp(int amount)
        {
            Mp -= amount;
            Update?.Invoke(nameof(Mp));
        }
    }
}
