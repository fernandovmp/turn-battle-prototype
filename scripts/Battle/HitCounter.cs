using System.Diagnostics;
using Godot;

namespace Rpg2d.Battle
{
    public class HitCounter
    {
        private long _lastHit = 0;
        private int _count = 0;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly int _millisecondsTolerance;

        public HitCounter(int millisecondsTolerance)
        {
            _millisecondsTolerance = millisecondsTolerance;
        }

        public int Hits => _count;

        public void Init()
        {
            _stopwatch.Restart();
        }

        public int CountHit()
        {
            long currentTime = _stopwatch.ElapsedMilliseconds;
            if (currentTime < _millisecondsTolerance || _count == 0)
            {
                _count++;
            }
            else
            {
                _count = 1;
            }
            _stopwatch.Restart();
            return _count;
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }
    }
}