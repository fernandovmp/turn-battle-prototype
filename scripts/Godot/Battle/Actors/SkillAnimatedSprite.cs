using System;
using Godot;
using Rpg2d.Godot.Skills;

namespace Rpg2d.Godot.Battle.Actors
{
    public class SkillAnimatedSprite : AnimatedSprite
    {
        public Action<int> FrameChanged { get; set; }
        public void Play(SkillAnimation animation)
        {
            Connect(Constants.AnimationFrameEnd, this, nameof(OnFrame));
            Connect(Constants.AnimationFinishedSignal, this, nameof(ResetSkillAnimation));
            Frames = animation.Frames;
            Scale = animation.CustomScale;
            Play(animation.Animation);
        }

        private void ResetSkillAnimation()
        {
            Disconnect(Constants.AnimationFrameEnd, this, nameof(OnFrame));
            Disconnect(Constants.AnimationFinishedSignal, this, nameof(ResetSkillAnimation));
            GetParent().RemoveChild(this);
        }

        private void OnFrame()
        {
            FrameChanged?.Invoke(Frame);
        }
    }
}
