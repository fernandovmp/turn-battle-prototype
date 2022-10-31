using Godot;
using Rpg2d.Skills;
using System;

namespace Rpg2d.Battle
{
    public abstract class BaseBattler : IBattler
    {
        public string Name { get; set; }
        public int MaxHp { get; set; }
        public int Hp { get; set; }
        public int Attack { get; set; }
        public SpriteFrames AnimationFrames { get; set; }
        public Skill AttackSkill { get; set; }
        public Action DamageRecived { get; set; }
        public Action Died { get; set; }

        public void DealDamage(int damage)
        {
            Hp -= damage;
            if (Hp <= 0)
            {
                Hp = 0;
                Died?.Invoke();
            }
            DamageRecived?.Invoke();
        }
    }
}
