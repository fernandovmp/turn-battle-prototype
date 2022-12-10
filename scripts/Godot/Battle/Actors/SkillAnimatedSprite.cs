using System;
using Godot;
using Rpg2d.Godot.Skills;

namespace Rpg2d.Godot.Battle.Actors
{
    public class SkillAnimatedSprite : AnimatedSprite
    {
        private AudioStreamPlayer _audioPlayer;
        public SkillAnimatedSprite() : base()
        {
            _audioPlayer = new AudioStreamPlayer();
            _audioPlayer.VolumeDb = -10;
            AddChild(_audioPlayer);
        }

        public Action<int> FrameChanged { get; set; }
        public void Play(SkillAnimation animation)
        {
            _audioPlayer.Stream = animation.HitEffect;
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
            QueueFree();
        }

        private void OnFrame()
        {
            FrameChanged?.Invoke(Frame);
        }

        internal void OnHit()
        {
            _audioPlayer.Play();
        }
    }
}
