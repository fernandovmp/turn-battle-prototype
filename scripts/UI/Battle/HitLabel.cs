using Godot;

namespace TurnBattle.UI.Battle
{
    public class HitLabel : Label
    {
        private float _time;

        public void ShowHitCount(int hits)
        {
            _time = 0;
            Text = $"{hits} HITS";
            Visible = true;
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            if (Visible)
            {
                _time += delta;
                if (_time > 1.4f)
                {
                    Visible = false;
                }
            }
        }
    }
}
