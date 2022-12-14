using System;

namespace TurnBattle.Battle.Actors
{
    public interface IBattler
    {
        string Name { get; }
        int MaxHp { get; }
        int Hp { get; set; }
        int Attack { get; }

        Action<BattlerDamageRecivedArgs> DamageRecived { get; set; }
        Action Died { get; set; }


        void DealDamage(int damage);
    }
}
