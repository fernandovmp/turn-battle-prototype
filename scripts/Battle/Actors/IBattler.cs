using System;
using TurnBattle.Battle.Stats;

namespace TurnBattle.Battle.Actors
{
    public interface IBattler
    {
        string Name { get; }
        RangeStat Hp { get; set; }
        RangeStat Mp { get; set; }
        int Attack { get; }
        Action<string> Update { get; set; }
        Action<BattlerDamageRecivedArgs> DamageRecived { get; set; }
        Action Died { get; set; }

        void DealDamage(int damage);
        void UseMp(int amount);
    }
}
