using Godot;

namespace TurnBattle.UI.Battle
{
    public class DamageLabel : Label
    {
        private float _speed = 100;
        float _x = 0;
        float _y = 0;
        private Vector2 _startPosition;

        public async void DestroyAfter(float seconds)
        {
            await ToSignal(GetTree().CreateTimer(seconds), "timeout");
            QueueFree();
        }

        public void SetStartPosition(Vector2 position)
        {
            _startPosition = position;
            RectGlobalPosition = position;
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            _x += _speed * delta;
            float y = -0.05f * Mathf.Pow(_x, 2) + 1.4f * _x;
            y = -Mathf.Max(0, y);
            if (y != 0)
                RectGlobalPosition = _startPosition + new Vector2(_x, y);
        }
    }
}
