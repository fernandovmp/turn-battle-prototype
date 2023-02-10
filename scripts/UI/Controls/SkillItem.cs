using System;
using Godot;

namespace TurnBattle.UI.Controls
{
    public class SkillItem : Button
    {
        public SkillItem(TurnBattle.Skills.Skill skill)
        {
            Skill = skill;
        }

        public TurnBattle.Skills.Skill Skill { get; private set; }
        public Action<TurnBattle.Skills.Skill> OnPressed { get; set; }

        public override void _Ready()
        {
            Text = Skill.Name;
        }

        public override void _Pressed()
        {
            base._Pressed();
            OnPressed?.Invoke(Skill);
        }
    }
}