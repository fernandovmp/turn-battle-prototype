using Godot;
using TurnBattle.Battle.AI;
using TurnBattle.Godot.Battle.AI;

namespace TurnBattle.Godot.Battle.Actors
{
    public class EnemyTroopResource : Resource
    {
        [Export]
        public string Name { get; set; }
        [Export]
        public EnemyResource[] Enemies { get; set; }
        [Export]
        public Vector2[] Positions { get; set; }
        [Export]
        public AiResource AI { get; set; }
    }
}
