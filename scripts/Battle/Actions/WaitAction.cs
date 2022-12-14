using TurnBattle.Battle.Actions;

namespace TurnBattle.Battle.Actions
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