using System;

namespace TurnBattle.Battle.Stats
{
    public struct RangeStat
    {
        private int _maxValue;
        private int _currentValue;

        public RangeStat(int maxValue) : this(maxValue, maxValue)
        {
        }

        public RangeStat(int maxValue, int currentValue)
        {
            _maxValue = maxValue;
            _currentValue = currentValue;
        }

        public int Max => _maxValue;
        public int Current=> _currentValue;

        public static RangeStat operator +(RangeStat stat, int value)
        {
            int newValue = Math.Max(Math.Min(stat._currentValue + value, stat._maxValue), 0);
            return new RangeStat(stat._maxValue, newValue);
        }

        public static RangeStat operator -(RangeStat stat, int value) => stat + (-value);

        public static bool operator ==(RangeStat stat, int value) => stat.Current == value;
        public static bool operator !=(RangeStat stat, int value) => stat.Current != value;
        public static bool operator >(RangeStat stat, int value) => stat.Current > value;
        public static bool operator <(RangeStat stat, int value) => stat.Current < value;
        public static bool operator >=(RangeStat stat, int value) => stat.Current >= value;
        public static bool operator <=(RangeStat stat, int value) => stat.Current <= value;

        
        public override bool Equals(object obj)
        {
            return obj is RangeStat stat &&
                   _maxValue == stat._maxValue &&
                   _currentValue == stat._currentValue &&
                   Max == stat.Max &&
                   Current == stat.Current;
        }

        public double Ratio() => Current / (double) Max;

        public override int GetHashCode()
        {
            int hashCode = -724538864;
            hashCode = hashCode * -1521134295 + _maxValue.GetHashCode();
            hashCode = hashCode * -1521134295 + _currentValue.GetHashCode();
            return hashCode;
        }
    }
}
