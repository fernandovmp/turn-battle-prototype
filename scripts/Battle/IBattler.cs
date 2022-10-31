using System;

namespace Rpg2d.Battle
{
    public interface IBattler
    {
        string Name { get; }
        int MaxHp { get; }
        int Hp { get; set; }
        int Attack { get; }

        Action DamageRecived { get; set; }
        Action Died { get; set; }


        void DealDamage(int damage);
    }
}
