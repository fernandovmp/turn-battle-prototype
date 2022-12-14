namespace TurnBattle.Battle.Actors
{
    public class SlotDamageRecivedArgs
    {
        public SlotDamageRecivedArgs(IBattlerSlot slot, int damage, int hitCount)
        {
            Slot = slot;
            Damage = damage;
            HitCount = hitCount;
        }

        public IBattlerSlot Slot { get; }
        public int Damage { get; }
        public int HitCount { get; }
    }
}