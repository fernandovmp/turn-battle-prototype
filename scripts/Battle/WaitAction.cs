namespace Rpg2d.Battle
{
    public class WaitAction : BattleAction
    {
        public WaitAction(float seconds)
        {
            Seconds = seconds;
        }
        public float Seconds { get; }
    }
}