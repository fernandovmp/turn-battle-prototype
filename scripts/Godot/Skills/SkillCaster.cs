using System;
using System.Collections.Generic;
using System.Linq;
using TurnBattle.Skills;
using Godot;

namespace TurnBattle.Godot.Skills
{
    public class SkillCaster
    {
        public SkillCaster(CastContext context)
        {
            Context = context;
            HitFrames = context.Skill.Animation.HitFrames.ToList().AsEnumerable<int>().GetEnumerator();
            HasFrames = HitFrames.MoveNext();
        }

        public CastContext Context { get; }
        public IEnumerator<int> HitFrames { get; }
        public bool HasFrames { get; private set; }
        public Action Hited { get; set; }

        public void OnFrame(int i)
        {
            if (HitFrames.Current == i)
            {
                Context.Skill.Cast(Context);
                Hited?.Invoke();
                HasFrames = HitFrames.MoveNext();
            }
        }
    }
}
