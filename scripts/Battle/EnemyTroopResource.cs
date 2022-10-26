using Godot;

namespace Rpg2d.Battle
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
