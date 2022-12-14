using Godot;
using TurnBattle.Godot.Battle.Actors;

namespace TurnBattle.Godot.Quests
{
    public class QuestResource : Resource
    {
        [Export]
        public string Name { get; set; }
        [Export]
        public EnemyTroopResource Troop { get; set; }
        [Export]
        public Texture Background { get; set; }
        [Export]
        public AudioStream BattleMusic { get; set; }
    }
}