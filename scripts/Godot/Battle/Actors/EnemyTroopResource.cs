using Godot;
using Rpg2d.Battle.AI;
using Rpg2d.Godot.Battle.AI;

namespace Rpg2d.Godot.Battle.Actors
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
