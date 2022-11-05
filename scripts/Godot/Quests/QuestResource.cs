using Godot;
using Rpg2d.Godot.Battle.Actors;

namespace Rpg2d.Godot.Quests
{
    public class QuestResource : Resource
    {
        [Export]
        public string Name { get; set; }
        [Export]
        public EnemyTroopResource Troop { get; set; }
    }
}