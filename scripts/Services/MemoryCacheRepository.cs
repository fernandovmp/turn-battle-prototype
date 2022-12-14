using System;
using System.Collections.Generic;

namespace TurnBattle.Services
{
    public class MemoryCacheRepository
    {
        private static IDictionary<string, object> _cache = new Dictionary<string, object>();
        public void SetValue<T>(string key, T value)
        {
            Remove(key);
            _cache.Add(key, value);
        }

        private void Remove(string key)
        {
            if (_cache.ContainsKey(key))
            {
                _cache.Remove(key);
            }
        }

        public T GetValue<T>(string key)
        {
            bool hasKey = _cache.TryGetValue(key, out object value);
            if (!hasKey)
            {
                throw new Exception($"Key'${key}' not found");
            }
            if (value is T tValue)
            {
                return tValue;
            }
            throw new Exception($"Value in key '{key}' is not of type {typeof(T).FullName}");
        }
    }
}