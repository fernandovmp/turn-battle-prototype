using Godot;
using Rpg2d.Battle;

namespace Rpg2d.Quests
{
    public class QuestResource : Resource
    {
        [Export]
        public string Name { get; set; }
        [Export]
        public EnemyTroopResource Troop { get; set; }
    }
}