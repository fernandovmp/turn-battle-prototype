using Rpg2d.Battle.Actions;

namespace Rpg2d.Battle.Actions
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